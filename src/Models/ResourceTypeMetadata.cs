using System.Text.Json.Serialization;

namespace McpServer.Models;

public class ResourceTypeMetadata
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    [JsonPropertyName("count")]
    public int Count { get; set; }
    
    [JsonPropertyName("validStatuses")]
    public string[] ValidStatuses { get; set; } = Array.Empty<string>();
}
