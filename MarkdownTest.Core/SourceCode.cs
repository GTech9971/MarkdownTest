using System.Text.Json.Serialization;

namespace MarkdownTest.Core;

/// <summary>
/// ソースコード詳細
/// </summary>
public class SourceCode
{
    [JsonRequired]
    [JsonPropertyName("namespace")]
    public string Namespace { get; init; } = null!;

    [JsonRequired]
    [JsonPropertyName("className")]
    public string ClassName { get; init; } = null!;

    [JsonRequired]
    [JsonPropertyName("isTest")]
    public bool IsTest { get; init; }

    [JsonPropertyName("sourceCodeDetails")]
    public IEnumerable<SourceCodeDetail> SourceCodeDetails = [];
}
