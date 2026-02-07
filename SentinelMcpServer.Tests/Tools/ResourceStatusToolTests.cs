using FluentAssertions;
using SentinelMcpServer.Tools;
using NUnit.Framework;

namespace SentinelMcpServer.Tests.Tools;

[TestFixture]
public class ResourceStatusToolTests
{
    [Test]
    public async Task GetResourceStatus_NoFilters_ShouldReturn100Resources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus();

        // Assert
        result.Count.Should().Be(100);
        result.Resources.Should().HaveCount(100);
    }

    [Test]
    public async Task GetResourceStatus_FilterByWorkerType_ShouldReturn60Workers()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(resourceType: "worker");

        // Assert
        result.Count.Should().Be(60);
        result.Resources.Should().HaveCount(60);
        result.Resources.Should().OnlyContain(r => r.Type == "worker");
    }

    [Test]
    public async Task GetResourceStatus_FilterByStorageBinType_ShouldReturn30StorageBins()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(resourceType: "storage-bin");

        // Assert
        result.Count.Should().Be(30);
        result.Resources.Should().HaveCount(30);
        result.Resources.Should().OnlyContain(r => r.Type == "storage-bin");
    }

    [Test]
    public async Task GetResourceStatus_FilterByTransporterType_ShouldReturn10Transporters()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(resourceType: "transporter");

        // Assert
        result.Count.Should().Be(10);
        result.Resources.Should().HaveCount(10);
        result.Resources.Should().OnlyContain(r => r.Type == "transporter");
    }

    [Test]
    public async Task GetResourceStatus_FilterByResourceType_CaseInsensitive_Worker()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(resourceType: "WORKER");

        // Assert
        result.Count.Should().Be(60);
        result.Resources.Should().HaveCount(60);
        result.Resources.Should().OnlyContain(r => r.Type == "worker");
    }

    [Test]
    public async Task GetResourceStatus_FilterByResourceType_CaseInsensitive_StorageBin()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(resourceType: "Storage-Bin");

        // Assert
        result.Count.Should().Be(30);
        result.Resources.Should().HaveCount(30);
        result.Resources.Should().OnlyContain(r => r.Type == "storage-bin");
    }

    [Test]
    public async Task GetResourceStatus_FilterByStatus_Active_ShouldReturnOnlyActiveResources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(status: "active");

        // Assert
        result.Resources.Should().OnlyContain(r => r.Status == "active");
        result.Count.Should().Be(result.Resources.Count);
        // Active is only valid for workers, so count should be <= 60
        result.Count.Should().BeLessOrEqualTo(60);
    }

    [Test]
    public async Task GetResourceStatus_FilterByStatus_Ready_ShouldReturnOnlyReadyResources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(status: "ready");

        // Assert
        result.Resources.Should().OnlyContain(r => r.Status == "ready");
        result.Count.Should().Be(result.Resources.Count);
        // Ready is only valid for workers, so count should be <= 60
        result.Count.Should().BeLessOrEqualTo(60);
    }

    [Test]
    public async Task GetResourceStatus_FilterByStatus_Maintenance_ShouldReturnOnlyMaintenanceResources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(status: "maintenance");

        // Assert
        result.Resources.Should().OnlyContain(r => r.Status == "maintenance");
        result.Count.Should().Be(result.Resources.Count);
        // Maintenance is only valid for workers, so count should be <= 60
        result.Count.Should().BeLessOrEqualTo(60);
    }

    [Test]
    public async Task GetResourceStatus_FilterByStatus_Empty_ShouldReturnOnlyEmptyResources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(status: "empty");

        // Assert
        result.Resources.Should().OnlyContain(r => r.Status == "empty");
        result.Count.Should().Be(result.Resources.Count);
        // Empty is only valid for storage bins, so count should be <= 30
        result.Count.Should().BeLessOrEqualTo(30);
    }

    [Test]
    public async Task GetResourceStatus_FilterByStatus_InUse_ShouldReturnOnlyInUseResources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(status: "in-use");

        // Assert
        result.Resources.Should().OnlyContain(r => r.Status == "in-use");
        result.Count.Should().Be(result.Resources.Count);
        // InUse is only valid for storage bins, so count should be <= 30
        result.Count.Should().BeLessOrEqualTo(30);
    }

    [Test]
    public async Task GetResourceStatus_FilterByStatus_Parked_ShouldReturnOnlyParkedResources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(status: "parked");

        // Assert
        result.Resources.Should().OnlyContain(r => r.Status == "parked");
        result.Count.Should().Be(result.Resources.Count);
        // Parked is only valid for transporters, so count should be <= 10
        result.Count.Should().BeLessOrEqualTo(10);
    }

    [Test]
    public async Task GetResourceStatus_FilterByStatus_CaseInsensitive()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(status: "ACTIVE");

        // Assert
        result.Resources.Should().OnlyContain(r => r.Status == "active");
        result.Count.Should().Be(result.Resources.Count);
    }

    [Test]
    public async Task GetResourceStatus_CombinedFilters_WorkerAndActive()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(
            resourceType: "worker", 
            status: "active");

        // Assert
        result.Resources.Should().OnlyContain(r => r.Type == "worker" && r.Status == "active");
        result.Count.Should().Be(result.Resources.Count);
        result.Count.Should().BeLessOrEqualTo(60);
    }

    [Test]
    public async Task GetResourceStatus_CombinedFilters_StorageBinAndEmpty()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(
            resourceType: "storage-bin", 
            status: "empty");

        // Assert
        result.Resources.Should().OnlyContain(r => r.Type == "storage-bin" && r.Status == "empty");
        result.Count.Should().Be(result.Resources.Count);
        result.Count.Should().BeLessOrEqualTo(30);
    }

    [Test]
    public async Task GetResourceStatus_CombinedFilters_TransporterAndParked()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(
            resourceType: "transporter", 
            status: "parked");

        // Assert
        result.Resources.Should().OnlyContain(r => r.Type == "transporter" && r.Status == "parked");
        result.Count.Should().Be(result.Resources.Count);
        result.Count.Should().BeLessOrEqualTo(10);
    }

    [Test]
    public async Task GetResourceStatus_InvalidResourceType_ShouldReturnZeroResources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(resourceType: "invalid-type");

        // Assert
        result.Count.Should().Be(0);
        result.Resources.Should().BeEmpty();
    }

    [Test]
    public async Task GetResourceStatus_InvalidStatus_ShouldReturnZeroResources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(status: "invalid-status");

        // Assert
        result.Count.Should().Be(0);
        result.Resources.Should().BeEmpty();
    }

    [Test]
    public async Task GetResourceStatus_InvalidCombinedFilters_WorkerAndEmpty_ShouldReturnZero()
    {
        // Act - workers can't have "empty" status (that's for storage bins)
        var result = await ResourceStatusTool.GetResourceStatus(
            resourceType: "worker", 
            status: "empty");

        // Assert
        result.Count.Should().Be(0);
        result.Resources.Should().BeEmpty();
    }

    [Test]
    public async Task GetResourceStatus_EmptyStringResourceType_ShouldReturnAll100Resources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(resourceType: "");

        // Assert
        result.Count.Should().Be(100);
        result.Resources.Should().HaveCount(100);
    }

    [Test]
    public async Task GetResourceStatus_EmptyStringStatus_ShouldReturnAll100Resources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(status: "");

        // Assert
        result.Count.Should().Be(100);
        result.Resources.Should().HaveCount(100);
    }

    [Test]
    public async Task GetResourceStatus_WhitespaceResourceType_ShouldReturnAll100Resources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(resourceType: "   ");

        // Assert
        result.Count.Should().Be(100);
        result.Resources.Should().HaveCount(100);
    }

    [Test]
    public async Task GetResourceStatus_WhitespaceStatus_ShouldReturnAll100Resources()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(status: "   ");

        // Assert
        result.Count.Should().Be(100);
        result.Resources.Should().HaveCount(100);
    }

    [Test]
    public async Task GetResourceStatus_CountOnlyTrue_ShouldReturnEmptyResourcesList()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(countOnly: true);

        // Assert
        result.Count.Should().Be(100);
        result.Resources.Should().BeEmpty();
    }

    [Test]
    public async Task GetResourceStatus_CountOnlyTrue_WithFilter_ShouldReturnEmptyResourcesList()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(
            resourceType: "worker", 
            countOnly: true);

        // Assert
        result.Count.Should().Be(60);
        result.Resources.Should().BeEmpty();
    }

    [Test]
    public async Task GetResourceStatus_CountOnlyFalse_ShouldReturnFullResourcesList()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(countOnly: false);

        // Assert
        result.Count.Should().Be(100);
        result.Resources.Should().HaveCount(100);
    }

    [Test]
    public async Task GetResourceStatus_CountShouldMatchResourcesListCount()
    {
        // Act
        var result = await ResourceStatusTool.GetResourceStatus(resourceType: "worker");

        // Assert
        result.Count.Should().Be(result.Resources.Count);
    }

    [Test]
    public async Task GetResourceStatus_AcceptsCancellationToken()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        var result = await ResourceStatusTool.GetResourceStatus(ct: cts.Token);

        // Assert - should complete successfully
        result.Should().NotBeNull();
        result.Count.Should().Be(100);
    }
}
