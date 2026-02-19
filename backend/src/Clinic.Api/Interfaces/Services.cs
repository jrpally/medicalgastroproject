using Clinic.Api.DTOs;
using Clinic.Api.Models;

namespace Clinic.Api.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentEntity> CreateAsync(CreateAppointmentRequest request, CancellationToken ct);
    Task TransitionAsync(Guid appointmentId, string targetStatus, CancellationToken ct);
    Task<IReadOnlyCollection<SlotDto>> GetSlotsAsync(string providerId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct);
}

public interface IStudyService
{
    Task<StudyEntity> CreateAsync(CreateStudyRequest request, CancellationToken ct);
    Task PatchAsync(Guid studyId, object patch, string etag, CancellationToken ct);
    Task<StudyEntity?> GetAsync(Guid studyId, CancellationToken ct);
    Task<IReadOnlyCollection<StudyEntity>> QueryAsync(string patientId, string? type, DateTimeOffset? from, DateTimeOffset? to, CancellationToken ct);
    Task<object> GetTimelineAsync(string patientId, DateTimeOffset? from, DateTimeOffset? to, CancellationToken ct);
}

public interface IAttachmentService
{
    Task<object> InitializeUploadAsync(object request, CancellationToken ct);
    Task<object> UploadChunkAsync(object request, CancellationToken ct);
    Task<object> CompleteUploadAsync(object request, CancellationToken ct);
    Task LinkAsync(Guid studyId, Guid attachmentId, CancellationToken ct);
}

public interface IAiService
{
    Task EnqueueAnalysisAsync(Guid attachmentId, AnalyzeAttachmentRequest request, CancellationToken ct);
    Task<object> GetAnalysisAsync(Guid attachmentId, CancellationToken ct);
    Task<object> DraftReportJsonAsync(Guid studyId, CancellationToken ct);
}

public interface IReportService
{
    Task<object> RenderAsync(RenderReportRequest request, CancellationToken ct);
    Task FinalizeAsync(Guid reportId, string etag, CancellationToken ct);
}
