using System.Diagnostics;
using System.Threading.Tasks;
using MarkdownTest.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace MarkdownTest.Cli;

public class SourceCodeParser
{

    public static async Task<IEnumerable<SourceCodeRoot>> ParseSolution(string solutionFilePath)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(solutionFilePath, nameof(solutionFilePath));

        using var workspace = MSBuildWorkspace.Create();
        Solution solution = await workspace.OpenSolutionAsync(solutionFilePath);

        Debug.WriteLine(solution);
        ICollection<SourceCodeRoot> list = [];
        foreach (var project in solution.Projects)
        {
            string projectName = project.Name;
            foreach (var document in project.Documents)
            {
                Debug.WriteLine($"{project.Name} {document.Name}");

                if (await IsGeneratedCode(document)) { continue; }

                var syntaxTree = await document.GetSyntaxTreeAsync();
                string source = syntaxTree.ToString();
                SourceCodeRoot sourceCodeRoot = await Parse(source, document.FilePath!, projectName);
                list.Add(sourceCodeRoot);
            }
        }

        return list;
    }

    public static async Task<bool> IsGeneratedCode(Document document)
    {
        if (document.Name.Contains("Global")) { return true; }
        var root = await document.GetSyntaxRootAsync();
        return root?.DescendantNodes().OfType<AttributeSyntax>().Select(x => x.ToString()).Any(x => x.Contains("GeneratedCode") || x.Contains("Global")) ?? false;
    }


    public static async Task<SourceCodeRoot> Parse(string source, string fileName, string projectName)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(source, nameof(source));

        SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
        SyntaxNode root = tree.GetRoot();

        string namespaceVal;
        var namespaceDeclaration = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>();
        if (namespaceDeclaration.Any() == false)
        {
            var fileScopeNamespaceDeclaration = root.DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>();
            if (fileScopeNamespaceDeclaration.Any() == false) { throw new InvalidDataException(); }
            namespaceVal = fileScopeNamespaceDeclaration.Single().Name.ToString();
        }
        else
        {
            namespaceVal = namespaceDeclaration.Single().Name.ToString();
        }

        var classes = root.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax>();

        ICollection<SourceCode> sourceCodes = [];
        foreach (var classNode in classes)
        {
            ICollection<SourceCodeDetail> details = [];
            var methods = classNode.Members.OfType<Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax>();
            foreach (var method in methods)
            {
                ICollection<Annotation> annotations = [];
                foreach (var attribute in method.AttributeLists.SelectMany(x => x.Attributes))
                {
                    IDictionary<string, object?> properties = new Dictionary<string, object?>();
                    foreach (var argument in attribute.ArgumentList.Arguments)
                    {
                        string? property = argument.NameEquals?.Name.Identifier.Text;
                        if (property == null) { continue; }
                        string value = argument.Expression.ToString();
                        properties.Add(property, value);
                    }
                    Annotation annotation = new Annotation()
                    {
                        Name = attribute.Name.ToString(),
                        Properties = properties
                    };
                    annotations.Add(annotation);

                    Debug.WriteLine(annotation);
                }

                string? xmlComment = method
                                        .GetLeadingTrivia()
                                        .Select(x => x.GetStructure())
                                        .OfType<DocumentationCommentTriviaSyntax>()
                                        .FirstOrDefault()
                                        ?.ToFullString()
                                        .Trim();

                SourceCodeDetail sourceCodeDetail = new SourceCodeDetail()
                {
                    MethodName = method.Identifier.Text,
                    MethodComment = xmlComment,
                    TestAnnotations = annotations,
                    Position = new System.Drawing.Point()
                    {
                        X = method.GetLocation().GetLineSpan().StartLinePosition.Line,
                        Y = method.GetLocation().GetLineSpan().EndLinePosition.Line
                    }
                };
                details.Add(sourceCodeDetail);
            }

            SourceCode sourceCode = new SourceCode()
            {
                Namespace = namespaceVal,
                ClassName = classNode.Identifier.Text,
                SourceCodeDetails = details
            };
            sourceCodes.Add(sourceCode);
        }

        return new SourceCodeRoot()
        {
            FileName = fileName,
            ProjectName = projectName,
            SourceCodes = sourceCodes,
        };
    }
}
