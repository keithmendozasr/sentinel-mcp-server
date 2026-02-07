# Development Instructions for Sentinel MCP Server

This document outlines coding conventions and patterns to follow when working on this project. These guidelines help maintain consistency and ensure AI coding assistants produce code that matches the existing codebase style.

## Code Style & Syntax

### Collection Initialization
- **Use collection expressions `[]`** instead of `new()` or `new List<T>()`
  ```csharp
  // ✅ Correct
  public List<ResourceDto> Resources { get; set; } = [];
  public string[] ValidStatuses { get; set; } = [];
  
  // ❌ Avoid
  public List<ResourceDto> Resources { get; set; } = new();
  public string[] ValidStatuses { get; set; } = new string[0];
  ```

### String Initialization
- **Use `string.Empty`** instead of `""` for empty string defaults
  ```csharp
  // ✅ Correct
  public string Name { get; set; } = string.Empty;
  
  // ❌ Avoid
  public string Name { get; set; } = "";
  ```

### Namespace Declaration
- **Use file-scoped namespace declarations** (no braces)
  ```csharp
  // ✅ Correct
  namespace SentinelMcpServer.Models;
  
  public class ResourceDto { }
  
  // ❌ Avoid
  namespace SentinelMcpServer.Models
  {
      public class ResourceDto { }
  }
  ```

### Constructor Style
- **Use primary constructors** for classes with dependencies
  ```csharp
  // ✅ Correct
  public class SentinelResourceMonitor(Random? random = null) : IEnumerable<ResourceDto>
  {
      private readonly Random _random = random ?? new Random();
  }
  ```

## Naming Conventions

### DTOs (Data Transfer Objects)
- All DTOs must end with `Dto` suffix using PascalCase
- Response DTOs follow pattern: `<Purpose>ResponseDto`
- Examples: `ResourceDto`, `BackendVersionResponseDto`, `ResourceStatusResponseDto`

### Fields
- **Private fields**: Use underscore prefix (`_random`, `_workerStates`)
- **Static readonly fields**: Use underscore prefix for constants/configuration

### Parameters
- Use camelCase for method parameters
- Use descriptive names that indicate purpose

## JSON Serialization

### JSON Property Names
- **Always use `[JsonPropertyName("...")]`** with camelCase names
- Apply to all public properties in DTOs
  ```csharp
  public class ResourceDto
  {
      [JsonPropertyName("name")]
      public string Name { get; set; } = string.Empty;
      
      [JsonPropertyName("type")]
      public string Type { get; set; } = string.Empty;
  }
  ```

### DTO Structure
- DTOs should be simple property bags with no logic
- Use auto-properties with default initializers
- No constructor logic in DTOs

## MCP Tool Patterns

### Tool Classes
- All tool classes must be **static**
- Apply `[McpServerToolType]` attribute at class level
- Place in `Tools/` namespace
  ```csharp
  [McpServerToolType]
  public static class BackendVersionTool
  {
      // Tool methods here
  }
  ```

### Tool Methods
- All tool methods must be **static**
- Apply `[McpServerTool(Name = "...")]` with camelCase name
- Apply `[Description("...")]` for discoverability
- **Always include `CancellationToken`** as last parameter (with `default` or required)
- Return `Task<T>` even for synchronous operations (use `Task.FromResult()`)
  ```csharp
  [McpServerTool(Name = "getBackendVersion")]
  [Description("Returns the backend server version")]
  public static Task<BackendVersionResponseDto> GetBackendVersion(CancellationToken ct)
  {
      var result = new BackendVersionResponseDto { Version = "0.1.0" };
      return Task.FromResult(result);
  }
  ```

### Tool Parameters
- Apply `[Description("...")]` to parameters for documentation
- Use nullable types with defaults for optional filters: `string? resourceType = null`
- Boolean flags should have defaults: `bool countOnly = false`

## Testing Patterns

### Test Framework
- Use **NUnit** with `[TestFixture]` and `[Test]` attributes
- Use **FluentAssertions** for all assertions (`.Should()` syntax)

### Test Structure
- **Naming**: Use `MethodName_Scenario_ExpectedBehavior` pattern
- **Organization**: Follow AAA (Arrange-Act-Assert) pattern with comment separators
- **Structure**: Mirror source directory structure in test project
  ```csharp
  [TestFixture]
  public class BackendVersionToolTests
  {
      [Test]
      public async Task GetBackendVersion_ShouldReturnVersion()
      {
          // Arrange
          using var cts = new CancellationTokenSource();
  
          // Act
          var result = await BackendVersionTool.GetBackendVersion(cts.Token);
  
          // Assert
          result.Should().NotBeNull();
          result.Version.Should().NotBeNullOrEmpty();
      }
  }
  ```

### Testing Best Practices
- Use **seeded `Random` instances** for deterministic testing of randomness
- Create `CancellationTokenSource` with `using` statement
- Test both happy path and edge cases
- Test case-insensitive behavior where applicable

## Architecture & Organization

### Namespace Structure
- `Services/`: Business logic and service classes
- `Tools/`: MCP tool endpoints (static classes)
- `Models/`: DTOs and data models

### Service Implementation
- Services can be instance classes with dependencies
- Use dependency injection where appropriate
- Document services with XML comments

## String Handling

### String Comparisons
- Use `StringComparison.OrdinalIgnoreCase` for case-insensitive filter comparisons
  ```csharp
  if (r.Type.Equals(resourceType, StringComparison.OrdinalIgnoreCase))
  {
      // Filter logic
  }
  ```

### Nullable Strings
- Use `string?` for optional parameters
- Check with `!string.IsNullOrWhiteSpace(value)` before using

## Documentation

### XML Documentation
- Use XML comments (`///`) for public classes and methods
- Especially important for service classes
- Document parameters with `<param>` tags
  ```csharp
  /// <summary>
  /// The SentinelResourceMonitor generates statuses for 100 resources.
  /// </summary>
  /// <param name="random">Optional Random instance for generating resource states.</param>
  public class SentinelResourceMonitor(Random? random = null)
  ```

### MCP Descriptions
- Use `[Description("...")]` attributes on:
  - Tool methods (for AI agent discoverability)
  - Tool parameters (to guide proper usage)
- Write descriptions that explain **what** the tool does and **when** to use it

## Async/Await Patterns

### Tool Methods
- All tool methods must return `Task<T>`
- Use `Task.FromResult()` for synchronous operations
- Include `CancellationToken` parameter even if not used
- Mark methods as `async` only if they truly await operations

## Code Quality

### LINQ Usage
- Prefer LINQ for collections filtering and transformations
- Chain operations for readability
- Use `.ToList()` when materializing results

### Constants
- Define valid states/statuses as `static readonly` arrays
- Keep configuration values as constants at the top of classes
- Use meaningful names that indicate purpose

## Project-Specific Patterns

### Resource Generation
- Resources are dynamically generated, not stored
- Use deterministic randomness for testing (seeded `Random`)
- Resource naming follows pattern: `{type-prefix}-{number:D3}`

### Status Validation
- Each resource type has specific valid statuses
- Status filtering should be case-insensitive
- Document valid statuses in both code and tests
