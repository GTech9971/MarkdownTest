using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MarkdownTest.Cli;

public class SourceCodeParser
{
    public static void Parse(string source)
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


        foreach (var classNode in classes)
        {
            var methods = classNode.Members.OfType<Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax>();
            foreach (var method in methods)
            {

                var attributes = method.AttributeLists.SelectMany(al => al.Attributes).Select(x => x.Name.ToString());

                var line = method.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                Debug.WriteLine($"{namespaceVal}.{classNode.Identifier.Text}.{method.Identifier.Text}() {line}");
            }
        }
    }
}
