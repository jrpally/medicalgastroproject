using Clinic.Api.DTOs;
using Clinic.Api.Interfaces;

namespace Clinic.Api.Services;

public sealed class ReportService(IBlobStorageClient blobStorageClient) : IReportService
{
    public Task<object> RenderAsync(RenderReportRequest request, CancellationToken ct)
    {
        // TODO: Use Open XML SDK to merge deterministic template + draft JSON.
        return Task.FromResult<object>(new { reportId = Guid.NewGuid(), status = "Rendered" });
    }

    public Task FinalizeAsync(Guid reportId, string etag, CancellationToken ct)
    {
        // TODO: Lock report and persist immutable final artifact metadata.
        return Task.CompletedTask;
    }
}
