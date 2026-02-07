using System.ComponentModel;
using SentinelMcpServer.Models;
using ModelContextProtocol.Server;

namespace SentinelMcpServer.Tools;

[McpServerToolType]
public static class BackendVersionTool
{
    [McpServerTool(Name = "getBackendVersion")]
    [Description("Returns the backend server version")]
    public static Task<BackendVersionResponseDto> GetBackendVersion(CancellationToken ct)
    {
        var result = new BackendVersionResponseDto
        {
            Version = "0.1.0"
        };

        return Task.FromResult(result);
    }
}
