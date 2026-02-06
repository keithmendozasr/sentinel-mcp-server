using System.Text.Json;

namespace McpServer.Services;

public class McpServerService : BackgroundService
{
    private readonly JsonRpcHandler _handler;

    public McpServerService(JsonRpcHandler handler)
    {
        _handler = handler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Read JSON-RPC lines from stdin and write responses to stdout.
        while (!stoppingToken.IsCancellationRequested)
        {
            string? line;
            try
            {
                line = await Console.In.ReadLineAsync().ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            if (line == null)
            {
                // stdin closed
                break;
            }

            try
            {
                var response = await _handler.HandleAsync(line, stoppingToken).ConfigureAwait(false);
                if (response != null)
                {
                    Console.Out.WriteLine(response);
                    Console.Out.Flush();
                }
            }
            catch (Exception ex)
            {
                // Log to stderr to avoid polluting JSON-RPC stdout
                Console.Error.WriteLine($"Error handling request: {ex}");
            }
        }
    }
}
