using System.Text.Json.Serialization;

namespace SentinelMcpServer.Models;

public class BackendVersionResponseDto
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}
