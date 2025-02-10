using System;
using System.Text.Json.Serialization;

namespace MarkdownTest.Core;

public record class Annotation
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;

    public IDictionary<string, object?>? Properties { get; init; }
}
