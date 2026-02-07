using McpServer.Models;

namespace McpServer.Tools;

public class ResourceStatusTool
{
    public object ListTools()
    {
        return new
        {
            tools = new[]
            {
                new
                {
                    name = "getResourceStatus",
                    description = "Returns status of resources",
                    inputSchema = new Dictionary<string, object>
                    {
                        ["type"] = "object",
                        ["properties"] = new Dictionary<string, object>(),
                        ["required"] = new string[] { }
                    }
                }
            }
        };
    }

    public Task<object> GetResourceStatusAsync(CancellationToken ct)
    {
        // Hardcoded mock resources
        var resources = new List<ResourceDto>
        {
            new ResourceDto { Name = "res-1", Type = "generic", Status = "ready" },
            new ResourceDto { Name = "res-2", Type = "generic", Status = "active" }
        };

        var result = new
        {
            count = resources.Count,
            resources
        };

        return Task.FromResult<object>(result);
    }
}
