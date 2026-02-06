using System.Text.Json;
using System.Text.Json.Serialization;
using McpServer.Models;
using McpServer.Tools;

namespace McpServer.Services;

public class JsonRpcHandler
{
    private readonly ResourceStatusTool _resourceTool;
    private readonly JsonSerializerOptions _opts = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public JsonRpcHandler(ResourceStatusTool resourceTool)
    {
        _resourceTool = resourceTool;
    }

    public async Task<string?> HandleAsync(string jsonLine, CancellationToken ct)
    {
        JsonRpcRequest? req;
        try
        {
            req = JsonSerializer.Deserialize<JsonRpcRequest>(jsonLine, _opts);
        }
        catch (JsonException je)
        {
            var parseError = new JsonRpcResponse
            {
                Error = new JsonRpcError { Code = -32700, Message = "Parse error: " + je.Message },
                Id = null
            };
            return JsonSerializer.Serialize(parseError, _opts);
        }

        if (req == null || req.Jsonrpc != "2.0" || string.IsNullOrEmpty(req.Method))
        {
            var invalid = new JsonRpcResponse
            {
                Error = new JsonRpcError { Code = -32600, Message = "Invalid Request" },
                Id = req?.Id
            };
            return JsonSerializer.Serialize(invalid, _opts);
        }

        try
        {
            if (req.Method == "tools/list")
            {
                var tools = _resourceTool.ListTools();
                var resp = new JsonRpcResponse { Result = tools, Id = req.Id };
                return JsonSerializer.Serialize(resp, _opts);
            }

            if (req.Method == "tools/call")
            {
                // Expect params to be an object with name
                string? toolName = null;
                if (req.ParamsElement.HasValue && req.ParamsElement.Value.ValueKind == JsonValueKind.Object)
                {
                    var obj = req.ParamsElement.Value;
                    if (obj.TryGetProperty("name", out var p) && p.ValueKind == JsonValueKind.String)
                    {
                        toolName = p.GetString();
                    }
                }

                if (string.IsNullOrEmpty(toolName))
                {
                    return JsonSerializer.Serialize(new JsonRpcResponse
                    {
                        Error = new JsonRpcError { Code = -32602, Message = "Invalid params: missing tool name" },
                        Id = req.Id
                    }, _opts);
                }

                if (toolName == "getResourceStatus")
                {
                    var result = await _resourceTool.GetResourceStatusAsync(ct).ConfigureAwait(false);
                    return JsonSerializer.Serialize(new JsonRpcResponse { Result = result, Id = req.Id }, _opts);
                }

                return JsonSerializer.Serialize(new JsonRpcResponse
                {
                    Error = new JsonRpcError { Code = -32601, Message = "Method not found" },
                    Id = req.Id
                }, _opts);
            }

            return JsonSerializer.Serialize(new JsonRpcResponse
            {
                Error = new JsonRpcError { Code = -32601, Message = "Method not found" },
                Id = req.Id
            }, _opts);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Handler exception: {ex}");
            var err = new JsonRpcResponse { Error = new JsonRpcError { Code = -32603, Message = "Internal error" }, Id = req.Id };
            return JsonSerializer.Serialize(err, _opts);
        }
    }
}
