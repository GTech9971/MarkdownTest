using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarkdownTest.Core;

public record class TestCase
{
    /// <summary>
    /// テストケース名
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }
    /// <summary>
    /// テスト概要
    /// </summary>
    [JsonPropertyName("summary")]
    public string Summary { get; init; }

    /// <summary>
    /// 入力値
    /// </summary>
    [JsonPropertyName("inputs")]
    public IEnumerable<string> Inputs { get; init; }

    /// <summary>
    /// 期待値
    /// </summary>
    [JsonPropertyName("expectedResults")]
    public IEnumerable<string> ExpectedResults { get; init; }

    /// <summary>
    /// 前提条件
    /// </summary>
    [JsonPropertyName("preconditions")]
    public IEnumerable<string> Preconditions { get; init; }

    /// <summary>
    /// 実行手順
    /// </summary>
    [JsonPropertyName("steps")]
    public IEnumerable<string> Steps { get; init; }

    /// <summary>
    /// 実行環境
    /// </summary>
    [JsonPropertyName("executeEnvironments")]
    public IEnumerable<string>? ExecuteEnvironments { get; private set; }

    /// <summary>
    /// テストコードとのリンク
    /// </summary>
    [JsonPropertyName("testCodeLink")]
    public string? TestCodeLink { get; private set; }

    /// <summary>
    /// 実装コードとのリンク
    /// </summary>
    [JsonPropertyName("codeLink")]
    public string? CodeLink { get; private set; }

    /// <summary>
    /// テストID
    /// </summary>
    [JsonPropertyName("testId")]
    public string? TestId { get; private set; }

    public TestCase(string name, string summary, IEnumerable<string> inputs, IEnumerable<string> expectedResults, IEnumerable<string> preconditions, IEnumerable<string> steps)
    {
        Name = name;
        Summary = summary;
        Inputs = inputs;
        ExpectedResults = expectedResults;
        Preconditions = preconditions;
        Steps = steps;
    }

    public string ToJson()
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        return JsonSerializer.Serialize(this, options);
    }

    public class Builder
    {
        private TestCase? _target;

        public Builder(string name, string summary, IEnumerable<string> inputs, IEnumerable<string> expectedResults, IEnumerable<string> preconditions, IEnumerable<string> steps)
        {
            _target = new TestCase(name, summary, inputs, expectedResults, preconditions, steps);
        }

        public Builder WithExecuteEnvironments(IEnumerable<string>? executeEnvironments)
        {
            if (_target == null) { throw new NullReferenceException(); }
            _target.ExecuteEnvironments = executeEnvironments;
            return this;
        }

        public Builder WithTestCodeLink(string? testCodeLink)
        {
            if (_target == null) { throw new NullReferenceException(); }
            _target.TestCodeLink = testCodeLink;
            return this;
        }

        public Builder WithCodeLink(string? codeLink)
        {
            if (_target == null) { throw new NullReferenceException(); }
            _target.CodeLink = codeLink;
            return this;
        }

        public Builder WithTestId(string? testId)
        {
            if (_target == null) { throw new NullReferenceException(); }
            _target.TestId = testId;
            return this;
        }

        public TestCase Build()
        {
            if (_target == null) { throw new InvalidOperationException("ビルド済みです"); }

            TestCase result = _target;
            _target = null;

            return result;
        }
    }
}
