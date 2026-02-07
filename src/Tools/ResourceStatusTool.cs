using System.ComponentModel;
using SentinelMcpServer.Models;
using SentinelMcpServer.Services;
using ModelContextProtocol.Server;

namespace SentinelMcpServer.Tools;

[McpServerToolType]
public static class ResourceStatusTool
{
    [McpServerTool(Name = "getResourceStatus")]
    [Description("Returns status of Sentinel resources with optional type and status filters")]
    public static Task<ResourceStatusResponseDto> GetResourceStatus(
        [Description("Filter by resource type. Leave empty for all types")] string? resourceType = null,
        [Description("Filter by status. Leave empty for all statuses")] string? status = null,
        [Description("Return only the count without resource details")] bool countOnly = false,
        CancellationToken ct = default)
    {
        // Generate resources from SentinelResourceMonitor
        var monitor = new SentinelResourceMonitor();
        var allResources = monitor.AsEnumerable();

        // Apply filters with case-insensitive matching
        var filteredResources = allResources.AsEnumerable();
        
        if (!string.IsNullOrWhiteSpace(resourceType))
        {
            filteredResources = filteredResources.Where(r => 
                r.Type.Equals(resourceType, StringComparison.OrdinalIgnoreCase));
        }
        
        if (!string.IsNullOrWhiteSpace(status))
        {
            filteredResources = filteredResources.Where(r => 
                r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        var resourcesList = filteredResources.ToList();
        
        var result = new ResourceStatusResponseDto
        {
            Count = resourcesList.Count,
            Resources = countOnly ? new List<ResourceDto>() : resourcesList
        };

        return Task.FromResult(result);
    }
}
