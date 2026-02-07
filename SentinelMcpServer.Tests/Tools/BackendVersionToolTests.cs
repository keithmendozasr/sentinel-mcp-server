using FluentAssertions;
using SentinelMcpServer.Tools;
using NUnit.Framework;

namespace SentinelMcpServer.Tests.Tools;

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

    [Test]
    public async Task GetBackendVersion_ShouldReturn010Version()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        var result = await BackendVersionTool.GetBackendVersion(cts.Token);

        // Assert
        result.Version.Should().Be("0.1.0");
    }

    [Test]
    public async Task GetBackendVersion_MultipleInvocations_ShouldReturnConsistentVersion()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        var result1 = await BackendVersionTool.GetBackendVersion(cts.Token);
        var result2 = await BackendVersionTool.GetBackendVersion(cts.Token);

        // Assert
        result1.Version.Should().Be(result2.Version);
    }

    [Test]
    public async Task GetBackendVersion_AcceptsCancellationToken()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        var result = await BackendVersionTool.GetBackendVersion(cts.Token);

        // Assert - should complete successfully
        result.Should().NotBeNull();
    }
}
