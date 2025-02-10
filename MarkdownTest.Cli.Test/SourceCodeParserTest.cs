using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MarkdownTest.Core;

namespace MarkdownTest.Cli.Test;

public class SourceCodeParserTest
{
    [Fact(DisplayName = "1ソースの解析")]
    public async Task single_source()
    {
        string testCode =
        """
        namespace MarkdownTest.Cli.Test;

        /// <summary>
        /// サンプルのテスト
        /// </summary>
        public class Sample
        {

            /// <summary>
            /// テスト内容
            /// </summary>
            [Fact(DisplayName = "1ケースのテスト解析")]
            public void parse_single_test()
            {
                //
            }

            /// <summary>
            /// テスト内容
            /// A
            /// </summary>
            [Fact(DisplayName = "nullのテスト解析", Skip = "実施不可能")]
            public void parse_null_test()
            {

            }

            /// <summary>
            /// ２ケースのテスト内容
            /// </summary>
            /// <param name="context"></param>
            [Theory(DisplayName = "2ケースのテスト解析")]
            [InlineData("sample")]
            [InlineData("dummy")]
            public void parse_multi_test(string context)
            {

            }
        }
        """;

        await SourceCodeParser.Parse(testCode, "sample.cs", "MarkdownTest.Cli.Test");
    }

    [Fact(DisplayName = "1プロジェクト解析")]
    public async Task parse_single_project()
    {
        var solution = "../../../../SampleProject/SampleProject.sln";

        IEnumerable<SourceCodeRoot> roots = await SourceCodeParser.ParseSolution(solution);

        Assert.True(roots.Any());
    }
}
