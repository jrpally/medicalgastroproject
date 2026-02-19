namespace Clinic.Workstation.Storage;

public sealed class EncryptedFileStore
{
    public Task<string> SaveAsync(Stream content, string relativePath, CancellationToken ct)
    {
        // TODO: AES-GCM per-file encryption and metadata sidecar.
        return Task.FromResult(relativePath);
    }
}
