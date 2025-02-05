using System.Diagnostics;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using MarkdownTest.Core;

namespace MarkdownTest.Cli;

public class MarkdownParser
{
    public static TestCase Parse(string markdown)
    {
        var pipeline = new MarkdownPipelineBuilder().Build();
        var document = Markdown.Parse(markdown, pipeline);

        string testNameVal = null!;
        string testSummaryVal = null!;
        IEnumerable<string> inputsVal = null!;
        IEnumerable<string> expectedVal = null!;
        IEnumerable<string> preConditionsVal = null!;
        IEnumerable<string> stepsVal = null!;

        IEnumerable<string>? executeEnvironments = null;
        string? testCodeLink = null;
        string? codeLink = null;
        string? testId = null;

        foreach (var block in document)
        {

            if (block is Markdig.Syntax.ThematicBreakBlock breakBlock)
            {
                Debug.WriteLine(breakBlock.Content.ToString());
                continue;
            }

            if (block is Markdig.Syntax.HeadingBlock headingBlock)
            {
                var headingText = string.Concat(headingBlock.Inline?.Select(x => x.ToString()));

                // 次のブロックの内容を取得
                var nextBlockIndex = document.IndexOf(block) + 1;
                if (nextBlockIndex < document.Count)
                {
                    var nextBlock = document[nextBlockIndex];

                    if (nextBlock is Markdig.Syntax.ParagraphBlock paragraphBlock)
                    {
                        var content = string.Concat(paragraphBlock.Inline?.Select(x => x.ToString())).Trim();

                        // セクション名に応じてプロパティに割り当て
                        switch (headingText)
                        {
                            case "テスト名": //記載者に制約を持たせないために色々な文字でも対応できるようにする TestName,... 
                                testNameVal = content;
                                break;
                            case "テスト概要":
                                testSummaryVal = content;
                                break;
                            case "テストID":
                                testId = content;
                                break;
                        }
                    }
                    else if (nextBlock is Markdig.Syntax.ListBlock listBlock)
                    {
                        IEnumerable<string> items = listBlock
                                                        .SelectMany(x => x.Descendants<LiteralInline>())
                                                        .Select(x => x.ToString())
                                                        .ToList();


                        switch (headingText)
                        {
                            case "入力値":
                                inputsVal = items;
                                break;
                            case "期待値":
                                expectedVal = items;
                                break;
                            case "前提条件":
                                preConditionsVal = items;
                                break;
                            case "実行手順":
                                stepsVal = items;
                                break;
                            case "実行環境":
                                executeEnvironments = items;
                                break;
                            case "コードリンク":
                                testCodeLink = items.Where(x => x.Contains("テスト")).SingleOrDefault();
                                codeLink = items.Where(x => x.Contains("実装")).SingleOrDefault();
                                break;

                        }
                    }
                    else
                    {
                        Debug.WriteLine(nextBlock.ToString());
                    }
                }


            }
        }

        TestCase.Builder builder = new TestCase.Builder(testNameVal, testSummaryVal, inputsVal, expectedVal, preConditionsVal, stepsVal);
        TestCase testCase = builder
                            .WithExecuteEnvironments(executeEnvironments)
                            .WithTestCodeLink(testCodeLink)
                            .WithCodeLink(codeLink)
                            .WithTestId(testId)
                            .Build();
        return testCase;
    }

    public static TestCase Parse(MarkdownDocument document)
    {
        string testNameVal = null!;
        string testSummaryVal = null!;
        IEnumerable<string> inputsVal = null!;
        IEnumerable<string> expectedVal = null!;
        IEnumerable<string> preConditionsVal = null!;
        IEnumerable<string> stepsVal = null!;

        IEnumerable<string>? executeEnvironments = null;
        string? testCodeLink = null;
        string? codeLink = null;
        string? testId = null;

        foreach (var block in document)
        {
            if (block is Markdig.Syntax.HeadingBlock headingBlock)
            {
                var headingText = string.Concat(headingBlock.Inline?.Select(x => x.ToString()));

                // 次のブロックの内容を取得
                var nextBlockIndex = document.IndexOf(block) + 1;
                if (nextBlockIndex < document.Count)
                {
                    var nextBlock = document[nextBlockIndex];

                    if (nextBlock is Markdig.Syntax.ParagraphBlock paragraphBlock)
                    {
                        var content = string.Concat(paragraphBlock.Inline?.Select(x => x.ToString())).Trim();

                        // セクション名に応じてプロパティに割り当て
                        switch (headingText)
                        {
                            case "テスト名": //記載者に制約を持たせないために色々な文字でも対応できるようにする TestName,... 
                                testNameVal = content;
                                break;
                            case "テスト概要":
                                testSummaryVal = content;
                                break;
                            case "テストID":
                                testId = content;
                                break;
                        }
                    }
                    else if (nextBlock is Markdig.Syntax.ListBlock listBlock)
                    {
                        IEnumerable<string> items = listBlock
                                                        .SelectMany(x => x.Descendants<LiteralInline>())
                                                        .Select(x => x.ToString())
                                                        .ToList();


                        switch (headingText)
                        {
                            case "入力値":
                                inputsVal = items;
                                break;
                            case "期待値":
                                expectedVal = items;
                                break;
                            case "前提条件":
                                preConditionsVal = items;
                                break;
                            case "実行手順":
                                stepsVal = items;
                                break;
                            case "実行環境":
                                executeEnvironments = items;
                                break;
                            case "コードリンク":
                                testCodeLink = items.Where(x => x.Contains("テスト")).SingleOrDefault();
                                codeLink = items.Where(x => x.Contains("実装")).SingleOrDefault();
                                break;

                        }
                    }
                    else
                    {
                        Debug.WriteLine(nextBlock.ToString());
                    }
                }


            }
        }

        TestCase.Builder builder = new TestCase.Builder(testNameVal, testSummaryVal, inputsVal, expectedVal, preConditionsVal, stepsVal);
        TestCase testCase = builder
                            .WithExecuteEnvironments(executeEnvironments)
                            .WithTestCodeLink(testCodeLink)
                            .WithCodeLink(codeLink)
                            .WithTestId(testId)
                            .Build();
        return testCase;
    }

    public static IEnumerable<MarkdownDocument> SplitByThematicBreakBlock(string markdown)
    {
        var pipeline = new MarkdownPipelineBuilder().Build();
        var document = Markdown.Parse(markdown, pipeline);

        var docs = new List<List<string>>();
        var currentSection = new List<string>();

        foreach (Block? block in document)
        {
            string? blockContext = block.ToString();
            if (block == null || blockContext == null) { continue; }

            // ---で次のテストケース
            if (block is ThematicBreakBlock breakBlock)
            {
                if (currentSection.Any())
                {
                    docs.Add(currentSection);
                    currentSection = new List<string>();
                }
            }
            currentSection.Add(blockContext);
        }
        if (currentSection.Any()) { docs.Add(currentSection); }

        IEnumerable<MarkdownDocument> documents = docs
                                                    .Select(x => Markdown.Parse(string.Join(Environment.NewLine, x), pipeline));

        return documents;
    }

    public static IEnumerable<TestCase> Parses(string markdown)
    {
        var documents = SplitByThematicBreakBlock(markdown);

        IEnumerable<TestCase> results = documents
                                            .Select(x => Parse(x));

        return results;
    }
}
