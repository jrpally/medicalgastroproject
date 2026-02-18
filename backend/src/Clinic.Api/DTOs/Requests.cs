namespace Clinic.Api.DTOs;

public sealed record CreateAppointmentRequest(
    Guid AppointmentId,
    string PatientId,
    string ProviderId,
    DateTimeOffset StartUtc,
    DateTimeOffset EndUtc,
    string? Notes,
    string IdempotencyKey);

public sealed record CreateStudyRequest(
    Guid StudyId,
    string PatientId,
    Guid? EncounterId,
    string StudyType,
    string? OrderedBy,
    string? PerformedBy,
    DateTimeOffset DateTimePerformed,
    string? NarrativeJson,
    string? StructuredFindingsJson,
    string IdempotencyKey);

public sealed record AnalyzeAttachmentRequest(
    Guid AttachmentId,
    string TaskType,
    string Locale,
    string IdempotencyKey);

public sealed record RenderReportRequest(
    Guid DraftId,
    Guid StudyId,
    string TemplateId,
    string DraftJson,
    IReadOnlyCollection<Guid> AttachmentIds,
    string IdempotencyKey);
