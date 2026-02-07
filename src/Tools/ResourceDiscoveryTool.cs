using System.ComponentModel;
using McpServer.Models;
using McpServer.Services;
using ModelContextProtocol.Server;

namespace McpServer.Tools;

[McpServerToolType]
public static class ResourceDiscoveryTool
{
    [McpServerTool(Name = "getResourceTypes")]
    [Description("Discovers available Sentinel resource types, their valid statuses, and total counts. Use this to learn what resources can be queried.")]
    public static Task<ResourceDiscoveryResponseDto> GetResourceTypes(CancellationToken ct = default)
    {
        var metadata = SentinelResourceMonitor.GetResourceMetadata();
        
        var response = new ResourceDiscoveryResponseDto
        {
            TotalResources = SentinelResourceMonitor.TotalResourceCount,
            ResourceTypes = metadata
        };

        return Task.FromResult(response);
    }
}
