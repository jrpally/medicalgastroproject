using Clinic.Api.DTOs;
using Clinic.Api.Interfaces;
using Clinic.Api.Storage;

namespace Clinic.Api.Services;

public sealed class AiService(IQueuePublisher queuePublisher) : IAiService
{
    public async Task EnqueueAnalysisAsync(Guid attachmentId, AnalyzeAttachmentRequest request, CancellationToken ct)
    {
        await queuePublisher.PublishAsync("ai-analysis", new
        {
            attachmentId,
            request.TaskType,
            request.Locale,
            request.IdempotencyKey
        }, ct);
    }

    public Task<object> GetAnalysisAsync(Guid attachmentId, CancellationToken ct)
        => Task.FromResult<object>(new { attachmentId, status = "Pending", result = (object?)null });

    public Task<object> DraftReportJsonAsync(Guid studyId, CancellationToken ct)
        => Task.FromResult<object>(new
        {
            studyId,
            sections = new[] { "history", "findings", "impression", "plan" },
            disclaimers = new[] { "AI output is assistive and must be reviewed by a licensed clinician." }
        });
}
