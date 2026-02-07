using System.ComponentModel;
using McpServer.Models;
using ModelContextProtocol.Server;

namespace McpServer.Tools;

[McpServerToolType]
public static class BackendVersionTool
{
    [McpServerTool(Name = "getBackendVersion")]
    [Description("Returns the backend server version")]
    public static Task<BackendVersionResponseDto> GetBackendVersion(CancellationToken ct)
    {
        var result = new BackendVersionResponseDto
        {
            Version = "1.0.0"
        };

        return Task.FromResult(result);
    }
}
