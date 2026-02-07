using System.ComponentModel;
using McpServer.Models;
using ModelContextProtocol.Server;

namespace McpServer.Tools;

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
        // Hardcoded mock resources
        var allResources = new List<ResourceDto>
        {
            new ResourceDto { Name = "worker-001", Type = "worker", Status = "ready" },
            new ResourceDto { Name = "worker-002", Type = "worker", Status = "active" },
            new ResourceDto { Name = "transporter-101", Type = "transporter", Status = "recovering" },
            new ResourceDto { Name = "transporter-102", Type = "transporter", Status = "ready" },
            new ResourceDto { Name = "scanner-201", Type = "scanner", Status = "active" },
            new ResourceDto { Name = "scanner-202", Type = "scanner", Status = "ready" },
            new ResourceDto { Name = "worker-003", Type = "worker", Status = "recovering" }
        };

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
