using System.Drawing;
using System.Text.Json.Serialization;

namespace MarkdownTest.Core;

/// <summary>
/// ソースコード詳細
/// </summary>
public record class SourceCodeDetail
{
    [JsonRequired]
    [JsonPropertyName("methodName")]
    public string MethodName { get; init; } = null!;

    [JsonPropertyName("methodComment")]
    public string? MethodComment { get; init; }

    [JsonPropertyName("testAnnotations")]
    public IEnumerable<Annotation> TestAnnotations { get; init; } = [];

    [JsonRequired]
    [JsonPropertyName("position")]
    public Point Position { get; init; }
}
