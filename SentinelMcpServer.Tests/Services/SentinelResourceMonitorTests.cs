using FluentAssertions;
using SentinelMcpServer.Services;
using NUnit.Framework;

namespace SentinelMcpServer.Tests.Services;

[TestFixture]
public class SentinelResourceMonitorTests
{
    [Test]
    public void GetEnumerator_ShouldGenerate100Resources()
    {
        // Arrange
        var monitor = new SentinelResourceMonitor();

        // Act
        var resources = monitor.ToList();

        // Assert
        resources.Should().HaveCount(100);
    }

    [Test]
    public void GetEnumerator_ShouldGenerate60Workers()
    {
        // Arrange
        var monitor = new SentinelResourceMonitor();

        // Act
        var resources = monitor.ToList();
        var workers = resources.Where(r => r.Type == "worker").ToList();

        // Assert
        workers.Should().HaveCount(60);
    }

    [Test]
    public void GetEnumerator_ShouldGenerate30StorageBins()
    {
        // Arrange
        var monitor = new SentinelResourceMonitor();

        // Act
        var resources = monitor.ToList();
        var storageBins = resources.Where(r => r.Type == "storage-bin").ToList();

        // Assert
        storageBins.Should().HaveCount(30);
    }

    [Test]
    public void GetEnumerator_ShouldGenerate10Transporters()
    {
        // Arrange
        var monitor = new SentinelResourceMonitor();

        // Act
        var resources = monitor.ToList();
        var transporters = resources.Where(r => r.Type == "transporter").ToList();

        // Assert
        transporters.Should().HaveCount(10);
    }

    [Test]
    public void GetEnumerator_WorkersShouldHaveCorrectNamingPattern()
    {
        // Arrange
        var monitor = new SentinelResourceMonitor();

        // Act
        var resources = monitor.ToList();
        var workers = resources.Where(r => r.Type == "worker").ToList();

        // Assert
        workers.Should().HaveCount(60);
        workers[0].Name.Should().Be("worker-001");
        workers[29].Name.Should().Be("worker-030");
        workers[59].Name.Should().Be("worker-060");
    }

    [Test]
    public void GetEnumerator_StorageBinsShouldHaveCorrectNamingPattern()
    {
        // Arrange
        var monitor = new SentinelResourceMonitor();

        // Act
        var resources = monitor.ToList();
        var storageBins = resources.Where(r => r.Type == "storage-bin").ToList();

        // Assert
        storageBins.Should().HaveCount(30);
        storageBins[0].Name.Should().Be("bin-001");
        storageBins[14].Name.Should().Be("bin-015");
        storageBins[29].Name.Should().Be("bin-030");
    }

    [Test]
    public void GetEnumerator_TransportersShouldHaveCorrectNamingPattern()
    {
        // Arrange
        var monitor = new SentinelResourceMonitor();

        // Act
        var resources = monitor.ToList();
        var transporters = resources.Where(r => r.Type == "transporter").ToList();

        // Assert
        transporters.Should().HaveCount(10);
        transporters[0].Name.Should().Be("transport-001");
        transporters[4].Name.Should().Be("transport-005");
        transporters[9].Name.Should().Be("transport-010");
    }

    [Test]
    public void GetEnumerator_WorkersShouldOnlyHaveValidStatuses()
    {
        // Arrange
        var random = new Random(42); // Seeded for deterministic testing
        var monitor = new SentinelResourceMonitor(random);
        var validStatuses = new[] { "active", "ready", "maintenance" };

        // Act
        var resources = monitor.ToList();
        var workers = resources.Where(r => r.Type == "worker").ToList();

        // Assert
        workers.Should().HaveCount(60);
        workers.Should().OnlyContain(w => validStatuses.Contains(w.Status));
    }

    [Test]
    public void GetEnumerator_StorageBinsShouldOnlyHaveValidStatuses()
    {
        // Arrange
        var random = new Random(42); // Seeded for deterministic testing
        var monitor = new SentinelResourceMonitor(random);
        var validStatuses = new[] { "empty", "in-use" };

        // Act
        var resources = monitor.ToList();
        var storageBins = resources.Where(r => r.Type == "storage-bin").ToList();

        // Assert
        storageBins.Should().HaveCount(30);
        storageBins.Should().OnlyContain(sb => validStatuses.Contains(sb.Status));
    }

    [Test]
    public void GetEnumerator_TransportersShouldOnlyHaveValidStatuses()
    {
        // Arrange
        var random = new Random(42); // Seeded for deterministic testing
        var monitor = new SentinelResourceMonitor(random);
        var validStatuses = new[] 
        { 
            "parked", 
            "in-transit-worker", 
            "in-transit-storage-bin", 
            "in-transit-worker-storage-bin" 
        };

        // Act
        var resources = monitor.ToList();
        var transporters = resources.Where(r => r.Type == "transporter").ToList();

        // Assert
        transporters.Should().HaveCount(10);
        transporters.Should().OnlyContain(t => validStatuses.Contains(t.Status));
    }

    [Test]
    public void GetEnumerator_WithSeededRandom_ShouldProduceDeterministicResults()
    {
        // Arrange
        var monitor1 = new SentinelResourceMonitor(new Random(123));
        var monitor2 = new SentinelResourceMonitor(new Random(123));

        // Act
        var resources1 = monitor1.ToList();
        var resources2 = monitor2.ToList();

        // Assert
        resources1.Should().HaveCount(100);
        resources2.Should().HaveCount(100);
        
        for (int i = 0; i < 100; i++)
        {
            resources1[i].Name.Should().Be(resources2[i].Name);
            resources1[i].Type.Should().Be(resources2[i].Type);
            resources1[i].Status.Should().Be(resources2[i].Status);
        }
    }

    [Test]
    public void GetEnumerator_WithDifferentSeeds_ShouldProduceDifferentStatuses()
    {
        // Arrange
        var monitor1 = new SentinelResourceMonitor(new Random(123));
        var monitor2 = new SentinelResourceMonitor(new Random(456));

        // Act
        var resources1 = monitor1.ToList();
        var resources2 = monitor2.ToList();

        // Assert - Names and types should match, but at least some statuses should differ
        resources1.Should().HaveCount(100);
        resources2.Should().HaveCount(100);
        
        var statusesDiffer = false;
        for (int i = 0; i < 100; i++)
        {
            resources1[i].Name.Should().Be(resources2[i].Name);
            resources1[i].Type.Should().Be(resources2[i].Type);
            if (resources1[i].Status != resources2[i].Status)
            {
                statusesDiffer = true;
            }
        }
        
        statusesDiffer.Should().BeTrue("different random seeds should produce different statuses");
    }

    [Test]
    public void GetEnumerator_MultipleEnumerations_ShouldGenerateDifferentStates()
    {
        // Arrange
        var monitor = new SentinelResourceMonitor(new Random());

        // Act
        var resources1 = monitor.ToList();
        var resources2 = monitor.ToList();

        // Assert - Structure should be same, but at least some statuses should differ
        resources1.Should().HaveCount(100);
        resources2.Should().HaveCount(100);
        
        var statusesDiffer = false;
        for (int i = 0; i < 100; i++)
        {
            resources1[i].Name.Should().Be(resources2[i].Name);
            resources1[i].Type.Should().Be(resources2[i].Type);
            if (resources1[i].Status != resources2[i].Status)
            {
                statusesDiffer = true;
            }
        }
        
        statusesDiffer.Should().BeTrue("multiple enumerations should produce different statuses");
    }

    [Test]
    public void GetEnumerator_AllResourcesShouldHaveStatusAssigned()
    {
        // Arrange
        var monitor = new SentinelResourceMonitor();

        // Act
        var resources = monitor.ToList();

        // Assert
        resources.Should().HaveCount(100);
        resources.Should().OnlyContain(r => !string.IsNullOrEmpty(r.Status));
    }

    [Test]
    public void GetResourceMetadata_ShouldReturn3ResourceTypes()
    {
        // Act
        var metadata = SentinelResourceMonitor.GetResourceMetadata();

        // Assert
        metadata.Should().HaveCount(3);
        metadata.Should().ContainKey("worker");
        metadata.Should().ContainKey("storage-bin");
        metadata.Should().ContainKey("transporter");
    }

    [Test]
    public void GetResourceMetadata_WorkerMetadata_ShouldBeCorrect()
    {
        // Act
        var metadata = SentinelResourceMonitor.GetResourceMetadata();

        // Assert
        metadata["worker"].Type.Should().Be("worker");
        metadata["worker"].Count.Should().Be(60);
        metadata["worker"].ValidStatuses.Should().BeEquivalentTo(new[] { "active", "ready", "maintenance" });
    }

    [Test]
    public void GetResourceMetadata_StorageBinMetadata_ShouldBeCorrect()
    {
        // Act
        var metadata = SentinelResourceMonitor.GetResourceMetadata();

        // Assert
        metadata["storage-bin"].Type.Should().Be("storage-bin");
        metadata["storage-bin"].Count.Should().Be(30);
        metadata["storage-bin"].ValidStatuses.Should().BeEquivalentTo(new[] { "empty", "in-use" });
    }

    [Test]
    public void GetResourceMetadata_TransporterMetadata_ShouldBeCorrect()
    {
        // Act
        var metadata = SentinelResourceMonitor.GetResourceMetadata();

        // Assert
        metadata["transporter"].Type.Should().Be("transporter");
        metadata["transporter"].Count.Should().Be(10);
        metadata["transporter"].ValidStatuses.Should().BeEquivalentTo(new[] 
        { 
            "parked", 
            "in-transit-worker", 
            "in-transit-storage-bin", 
            "in-transit-worker-storage-bin" 
        });
    }

    [Test]
    public void TotalResourceCount_ShouldBe100()
    {
        // Act
        var totalCount = SentinelResourceMonitor.TotalResourceCount;

        // Assert
        totalCount.Should().Be(100);
    }
}
