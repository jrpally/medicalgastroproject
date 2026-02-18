namespace Clinic.Api.Storage;

public interface ITableClientFactory
{
    object GetClient(string tableName);
}

public interface IBlobStorageClient
{
    Task<string> GetReadSasAsync(string blobName, TimeSpan ttl, CancellationToken ct);
}

public interface IQueuePublisher
{
    Task PublishAsync(string queueName, object payload, CancellationToken ct);
}
