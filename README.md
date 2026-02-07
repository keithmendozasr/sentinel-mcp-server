# Sentinel MCP Server

## Problem Statement
This project is an MCP server for "Sentinel" system. The goal is to allow an AI agent to get information about the state of resources that "Sentinel" monitors.
As an aside, this is intended for me to experience vibe coding and MCP Server creation.

## Goals
- Produce an MCP server using vibe-coding mostly
- Build a foundation for future MCP server development

## Requirements
### Functional
- The system shall act as a basis for a future MCP server
- The MCP server exposes tools to query resource status and backend information

### Non-Functional
- Performance: The system should be able to handle start processing at least 100 transactions per second; where each transaction can take up to 10 seconds to finish
- Security: For now, make the MCP server open access
- Be able to debug the MCP server code while it's attached to the IDE's agent

## Architecture Notes
- Language: C#
- Framework: .NET 10.0 (cross-platform)
- Libraries:
  - ModelContextProtocol (v0.8.0-preview.1)
  - Microsoft.Extensions.Hosting (v8.0.0)

## Acceptance Criteria

### Tool: `getResourceStatus`
Returns the status of resources with optional filtering.

**Parameters:**
- `resourceType` (optional): Filter by resource type (e.g., "worker", "storage-bin", "transporter")
- `status` (optional): Filter by status
- `countOnly` (optional, default: false): Return only count without resource details

**Response structure:**
```json
{
  "count": <number of resources>,
  "resources": [
    {
      "name": "<resource name>",
      "type": "<resource type>",
      "status": "<resource status>"
    }
  ]
}
```

**Valid statuses by resource type:**
- `worker`: "active", "ready", "maintenance"
- `storage-bin`: "empty", "in-use"
- `transporter`: "parked", "in-transit-worker", "in-transit-storage-bin", "in-transit-worker-storage-bin"

### Tool: `getBackendVersion`
Returns the backend server version.

**Parameters:** None

**Response structure:**
```json
{
  "version": "<version string>"
}
```

### Tool: `getResourceTypes`
Returns metadata about available resource types, their counts, and valid statuses.

**Parameters:** None

**Response structure:**
```json
{
  "totalResources": <total count>,
  "resourceTypes": {
    "<type-name>": {
      "type": "<type-name>",
      "count": <count>,
      "validStatuses": ["<status1>", "<status2>"]
    }
  }
}
```

## Out of Scope
Do not build a web-based UI for this.

## Run (local)

Build and run the MCP server (worker) locally:

```cli
dotnet build src/SentinelMcpServer.csproj
dotnet run --project src/SentinelMcpServer.csproj
```

```MCP inspector
npx @modelcontextprotocol/inspector dotnet run --no-logo --no-build --project "<path to sentinel-mcp-server>/src/SentinelMcpServer.csproj"
```

The server reads JSON-RPC 2.0 messages from stdin and writes responses to stdout. Logging is written to stderr.