using System.Diagnostics;
using CommandLine;
using MarkdownTest.Core;

namespace MarkdownTest.Cli;

public class Client
{
    public class Options
    {
        [Option('c', "context", Required = true, HelpText = "markdownの内容")]
        public string Context { get; set; } = null!;
    }

    public static void Main(string[] args)
    {
        Parser.Default
                .ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    Console.WriteLine(options.Context);
                    TestCase testCase = MarkdownParser.Parse(options.Context);
                    Console.WriteLine(testCase.ToJson());
                    Debug.WriteLine(testCase.ToJson());
                });
    }
}
