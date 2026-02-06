using System.Text.Json;
using System.Text.Json.Serialization;

namespace McpServer.Models;

public class JsonRpcRequest
{
    [JsonPropertyName("jsonrpc")] public string? Jsonrpc { get; set; }
    [JsonPropertyName("method")] public string? Method { get; set; }
    [JsonPropertyName("params")] public JsonElement? ParamsElement { get; set; }
    [JsonPropertyName("id")] public JsonElement? Id { get; set; }
}

public class JsonRpcResponse
{
    [JsonPropertyName("jsonrpc")] public string Jsonrpc { get; set; } = "2.0";
    [JsonPropertyName("result")] public object? Result { get; set; }
    [JsonPropertyName("error")] public JsonRpcError? Error { get; set; }
    [JsonPropertyName("id")] public JsonElement? Id { get; set; }
}

public class JsonRpcError
{
    [JsonPropertyName("code")] public int Code { get; set; }
    [JsonPropertyName("message")] public string? Message { get; set; }
}
