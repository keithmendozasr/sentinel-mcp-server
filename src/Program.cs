using McpServer.Services;
using McpServer.Tools;

namespace McpServer;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddSingleton<JsonRpcHandler>();
                services.AddSingleton<ResourceStatusTool>();
                services.AddHostedService<McpServerService>();
            })
            .Build();

        await host.RunAsync();
    }
}
