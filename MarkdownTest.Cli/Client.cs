using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using CommandLine;
using MarkdownTest.Core;

namespace MarkdownTest.Cli;

public class Client
{
    public class Options
    {
        [Option('c', "context", HelpText = "markdownの内容")]
        public string? Context { get; set; }

        [Option('s', "solution", HelpText = "sln(ソリューションファイルパス)")]
        public string? Solution { get; set; }
    }

    public static async Task Main(string[] args)
    {
        await Parser.Default
                .ParseArguments<Options>(args)
                .WithParsedAsync(async options =>
                {
                    if (string.IsNullOrWhiteSpace(options.Solution) == false)
                    {
                        IEnumerable<SourceCodeRoot> sourceCodeRoots = await SourceCodeParser.ParseSolution(options.Solution);
                        var jsonOptions = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
                        string json = JsonSerializer.Serialize(sourceCodeRoots, jsonOptions);
                        Console.WriteLine(json);
                        Debug.WriteLine(json);
                    }
                    else
                    {
                        TestCase testCase = MarkdownParser.Parse(options.Context!);
                        Console.WriteLine(testCase.ToJson());
                    }
                });
    }
}
