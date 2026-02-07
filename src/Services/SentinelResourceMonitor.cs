using System.Collections;
using McpServer.Models;

namespace McpServer.Services;

/// <summary>
/// The SentinelResourceMonitor generates statuses for 100 resources.
/// - 60 "worker" resources
/// - 30 "storage-bin" resources
/// - 10 "transporter" resources
/// States are randomly generated each time the collection is enumerated.
/// </summary>
public class SentinelResourceMonitor : IEnumerable<ResourceDto>
{
    private static readonly Random _random = new();
    
    private static readonly string[] _workerStates = ["active", "ready", "maintenance"];
    private static readonly string[] _storageBinStates = ["empty", "in-use"];
    private static readonly string[] _transporterStates = 
    [ 
        "parked", 
        "in-transit-worker", 
        "in-transit-storage-bin", 
        "in-transit-worker-storage-bin" 
    ];

    private const int WorkerCount = 60;
    private const int StorageBinCount = 30;
    private const int TransporterCount = 10;

    /// <summary>
    /// Gets metadata about available resource types, their statuses, and counts.
    /// </summary>
    public static Dictionary<string, ResourceTypeMetadata> GetResourceMetadata()
    {
        return new Dictionary<string, ResourceTypeMetadata>
        {
            ["worker"] = new ResourceTypeMetadata
            {
                Type = "worker",
                Count = WorkerCount,
                ValidStatuses = _workerStates
            },
            ["storage-bin"] = new ResourceTypeMetadata
            {
                Type = "storage-bin",
                Count = StorageBinCount,
                ValidStatuses = _storageBinStates
            },
            ["transporter"] = new ResourceTypeMetadata
            {
                Type = "transporter",
                Count = TransporterCount,
                ValidStatuses = _transporterStates
            }
        };
    }

    public static int TotalResourceCount => WorkerCount + StorageBinCount + TransporterCount;

    public IEnumerator<ResourceDto> GetEnumerator()
    {
        // Generate 60 workers
        for (int i = 1; i <= WorkerCount; i++)
        {
            yield return new ResourceDto
            {
                Name = $"worker-{i:D3}",
                Type = "worker",
                Status = _workerStates[_random.Next(_workerStates.Length)]
            };
        }

        // Generate 30 storage bins
        for (int i = 1; i <= StorageBinCount; i++)
        {
            yield return new ResourceDto
            {
                Name = $"bin-{i:D3}",
                Type = "storage-bin",
                Status = _storageBinStates[_random.Next(_storageBinStates.Length)]
            };
        }

        // Generate 10 transporters
        for (int i = 1; i <= TransporterCount; i++)
        {
            yield return new ResourceDto
            {
                Name = $"transport-{i:D3}",
                Type = "transporter",
                Status = _transporterStates[_random.Next(_transporterStates.Length)]
            };
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}