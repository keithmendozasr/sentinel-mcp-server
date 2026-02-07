using System.Text.Json.Serialization;

namespace SentinelMcpServer.Models;

public class ResourceDiscoveryResponseDto
{
    [JsonPropertyName("totalResources")]
    public int TotalResources { get; set; }
    
    [JsonPropertyName("resourceTypes")]
    public Dictionary<string, ResourceTypeMetadata> ResourceTypes { get; set; } = [];
}
