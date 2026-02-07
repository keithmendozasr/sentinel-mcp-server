using System.Text.Json.Serialization;

namespace McpServer.Models;

public class ResourceStatusResponseDto
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("resources")]
    public List<ResourceDto> Resources { get; set; } = new();
}
