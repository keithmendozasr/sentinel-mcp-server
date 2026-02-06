# AI Stuff

## Problem Statement
This is a project to help me understand AI-based coding better

## Goals
- Produce an MCP server using vibe-coding mostly
- Have scaffolding in place to allow for the C#-based web server to call C++-based code for the fun of it

## Requirements
### Functional
- The system shall act as a basis for a future MCP server
- The web server will be in C#
- Have provisions so that the web controller can hand off processing to methods written in C++
- As a starting iteration the MCP server can send "hard-coded values"
- Have provisions to be able to test/run MCP server functionality on a command-line.

### Non-Functional
- Performance: The system should be able to handle start processing at least 100 transactions per second; where each transaction can take up to 10 seconds to finish
- Security: For now, make the MCP server open access

## Architecture Notes
- Language: C# for the web server. Option for processing to be written in C++
- Framework: .Net cross-platform

## Acceptance Criteria
- Given I ask the AI Agent for the status of resource A
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

## Out of Scope
Do not build a web-based UI for this.
