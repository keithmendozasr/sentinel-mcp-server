using System.Text.Json.Serialization;

namespace McpServer.Models;

public class BackendVersionResponseDto
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}
