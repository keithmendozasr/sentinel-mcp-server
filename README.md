# My Study MCP Server

## Problem Statement
This is a project to help me understand AI-based coding better

## Goals
- Produce an MCP server using vibe-coding mostly
- Build a foundation for future MCP server development

## Requirements
### Functional
- The system shall act as a basis for a future MCP server
- As a starting iteration the MCP server can send "hard-coded values"

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
- Given I ask the AI Agent for the status of Study resource A
  Then the MCP server will return a JSON with the following structure:
  ```
  {
    count: <Number of resources>,
    resources: [
        {
            name: <name of resource>,
            type: <type of resource>,
            status: <one of "ready", "recovering", "active>
        }
    ]
  }
  ```
- Given I ask the AI Agent for the backend version of Study server
  Then the MCP server will return a JSON with the following structure:
  ```
  {
    version: <Some version configured in code>
  }
  ```

## Out of Scope
Do not build a web-based UI for this.

## Run (local)

Build and run the MCP server (worker) locally:

```cli
dotnet build src/McpServer.csproj
dotnet run --project src/McpServer.csproj
```

```MCP inspector
npx @modelcontextprotocol/inspector pwsh -NoLogo -Command 'dotnet run --no-logo --no-build --project "<path to mcp-server>\src\McpServer.csproj"
```

The server reads JSON-RPC 2.0 messages from stdin and writes responses to stdout. Logging is written to stderr.

## Change log
- 2026-02-07
  - Added ModelContextProtocol SDK (v0.8.0-preview.1)
  - Removed C++ integration provisions from codebase
  - Implemented attribute-based tool registration using `[McpServerTool]`
  - Added `getResourceStatus` tool returning strongly-typed DTOs
  - Added `getBackendVersion` tool per acceptance criteria
  - Refactored to use SDK's stdio transport and automatic protocol handling
  - Removed manual JSON-RPC message handling (~200 lines of code eliminated)