using FluentAssertions;
using SentinelMcpServer.Tools;
using NUnit.Framework;

namespace SentinelMcpServer.Tests.Tools;

[TestFixture]
public class ResourceDiscoveryToolTests
{
    [Test]
    public async Task GetResourceTypes_ShouldReturnTotalResourceCount100()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        result.TotalResources.Should().Be(100);
    }

    [Test]
    public async Task GetResourceTypes_ShouldReturn3ResourceTypes()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        result.ResourceTypes.Should().HaveCount(3);
    }

    [Test]
    public async Task GetResourceTypes_ShouldContainWorkerType()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        result.ResourceTypes.Should().ContainKey("worker");
    }

    [Test]
    public async Task GetResourceTypes_ShouldContainStorageBinType()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        result.ResourceTypes.Should().ContainKey("storage-bin");
    }

    [Test]
    public async Task GetResourceTypes_ShouldContainTransporterType()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        result.ResourceTypes.Should().ContainKey("transporter");
    }

    [Test]
    public async Task GetResourceTypes_WorkerMetadata_ShouldBeCorrect()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        var workerMetadata = result.ResourceTypes["worker"];
        workerMetadata.Type.Should().Be("worker");
        workerMetadata.Count.Should().Be(60);
        workerMetadata.ValidStatuses.Should().BeEquivalentTo(["active", "ready", "maintenance"]);
    }

    [Test]
    public async Task GetResourceTypes_StorageBinMetadata_ShouldBeCorrect()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        var storageBinMetadata = result.ResourceTypes["storage-bin"];
        storageBinMetadata.Type.Should().Be("storage-bin");
        storageBinMetadata.Count.Should().Be(30);
        storageBinMetadata.ValidStatuses.Should().BeEquivalentTo(["empty", "in-use"]);
    }

    [Test]
    public async Task GetResourceTypes_TransporterMetadata_ShouldBeCorrect()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        var transporterMetadata = result.ResourceTypes["transporter"];
        transporterMetadata.Type.Should().Be("transporter");
        transporterMetadata.Count.Should().Be(10);
        transporterMetadata.ValidStatuses.Should().BeEquivalentTo(
        [
            "parked", 
            "in-transit-worker", 
            "in-transit-storage-bin", 
            "in-transit-worker-storage-bin" 
        ]);
    }

    [Test]
    public async Task GetResourceTypes_CountsShouldSumToTotalResources()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        var totalCount = result.ResourceTypes.Values.Sum(m => m.Count);
        totalCount.Should().Be(result.TotalResources);
        totalCount.Should().Be(100);
    }

    [Test]
    public async Task GetResourceTypes_AllMetadataTypesShouldMatchKeys()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        foreach (var kvp in result.ResourceTypes)
        {
            kvp.Value.Type.Should().Be(kvp.Key);
        }
    }

    [Test]
    public async Task GetResourceTypes_ValidStatusesShouldNotBeEmpty()
    {
        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        foreach (var metadata in result.ResourceTypes.Values)
        {
            metadata.ValidStatuses.Should().NotBeNullOrEmpty();
        }
    }

    [Test]
    public async Task GetResourceTypes_AcceptsCancellationToken()
    {
        // Arrange
        using var cts = new CancellationTokenSource();

        // Act
        var result = await ResourceDiscoveryTool.GetResourceTypes(cts.Token);

        // Assert - should complete successfully
        result.Should().NotBeNull();
        result.TotalResources.Should().Be(100);
    }

    [Test]
    public async Task GetResourceTypes_MultipleInvocations_ShouldReturnConsistentResults()
    {
        // Act
        var result1 = await ResourceDiscoveryTool.GetResourceTypes();
        var result2 = await ResourceDiscoveryTool.GetResourceTypes();

        // Assert
        result1.TotalResources.Should().Be(result2.TotalResources);
        result1.ResourceTypes.Should().BeEquivalentTo(result2.ResourceTypes);
    }
}
