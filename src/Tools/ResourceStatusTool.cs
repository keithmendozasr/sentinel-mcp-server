using System.ComponentModel;
using McpServer.Models;
using ModelContextProtocol.Server;

namespace McpServer.Tools;

[McpServerToolType]
public static class ResourceStatusTool
{
    [McpServerTool(Name = "getResourceStatus")]
    [Description("Returns status of resources")]
    public static Task<ResourceStatusResponseDto> GetResourceStatus(CancellationToken ct)
    {
        // Hardcoded mock resources
        var resources = new List<ResourceDto>
        {
            new ResourceDto { Name = "res-1", Type = "generic", Status = "ready" },
            new ResourceDto { Name = "res-2", Type = "generic", Status = "active" }
        };

        var result = new ResourceStatusResponseDto
        {
            Count = resources.Count,
            Resources = resources
        };

        return Task.FromResult(result);
    }
}
