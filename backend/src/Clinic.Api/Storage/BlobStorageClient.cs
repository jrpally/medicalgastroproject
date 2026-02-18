namespace Clinic.Api.Storage;

public sealed class BlobStorageClient : IBlobStorageClient
{
    public Task<string> GetReadSasAsync(string blobName, TimeSpan ttl, CancellationToken ct)
    {
        // TODO: build short-lived user delegation SAS.
        return Task.FromResult($"https://blob.example/{blobName}?sig=todo");
    }
}
