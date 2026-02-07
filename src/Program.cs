namespace McpServer;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        
        builder.Logging.AddConsole(opts => 
            opts.LogToStandardErrorThreshold = LogLevel.Trace);
        
        builder.Services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();

        await builder.Build().RunAsync();
    }
}
