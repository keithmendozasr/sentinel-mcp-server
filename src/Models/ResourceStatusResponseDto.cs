using System.Text.Json.Serialization;

namespace SentinelMcpServer.Models;

public class ResourceStatusResponseDto
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("resources")]
    public List<ResourceDto> Resources { get; set; } = [];
}
