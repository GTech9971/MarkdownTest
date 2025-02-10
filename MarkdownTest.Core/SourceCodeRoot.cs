using System.Text.Json.Serialization;

namespace MarkdownTest.Core;

/// <summary>
/// ソースコード
/// </summary>
public record class SourceCodeRoot
{
    [JsonRequired]
    [JsonPropertyName("fileName")]
    public string FileName { get; init; } = null!;

    [JsonRequired]
    [JsonPropertyName("projectName")]
    public string ProjectName { get; init; } = null!;

    [JsonPropertyName("sourceCodes")]
    public IEnumerable<SourceCode> SourceCodes { get; init; } = [];
}
