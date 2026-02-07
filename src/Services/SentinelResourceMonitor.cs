namespace McpServer.Services;

// The SentinelResourceMonitor will generate statuses for 100 resources using the McpServer.Models.ResourceDto model.
// The breakdown of the resources are as follows:
// - 30 "storage-bin" resources
// - 10 "transport" resources
// - The remainder are "worker" resources
// The resource types are "worker", "storage-bin", and "transporter".
// The "worker" resource can have the following states: "active", "ready", "maintenance"
// The "storage-bin" resource can have the following states: "empty", "in-use"
// The "transporter" resource can have the following states: "parked", "in-transit-worker", "in-transit-storage-bin", "in-transit-worker-storage-bin"
// The class should implement the IEnumeable interface and can effectively act on 'Where' clauses by the McpServer.Tools.ResourceStatusTool.GetResourceStatus() method
// The resource names should just be a sequential starting from one; i.e: "worker-001", "worker-002", "bin-001", "bin-002", "transport-001", "transport-002"
// The state of each resource should be randomly generated each time GetResourceStatus() requests data. Do not worry about the resources transitioning from one state to another.