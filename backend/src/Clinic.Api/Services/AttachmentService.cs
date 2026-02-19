using Clinic.Api.Interfaces;
using Clinic.Api.Storage;

namespace Clinic.Api.Services;

public sealed class AttachmentService(IBlobStorageClient blobStorageClient) : IAttachmentService
{
    public Task<object> InitializeUploadAsync(object request, CancellationToken ct)
        => Task.FromResult<object>(new { uploadSessionId = Guid.NewGuid(), chunkSizeBytes = 4 * 1024 * 1024 });

    public Task<object> UploadChunkAsync(object request, CancellationToken ct)
        => Task.FromResult<object>(new { accepted = true });

    public Task<object> CompleteUploadAsync(object request, CancellationToken ct)
        => Task.FromResult<object>(new { completed = true, attachmentId = Guid.NewGuid() });

    public Task LinkAsync(Guid studyId, Guid attachmentId, CancellationToken ct)
        => Task.CompletedTask;
}
