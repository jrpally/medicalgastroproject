namespace Clinic.Api.Models;

public sealed record AppointmentEntity(
    Guid AppointmentId,
    string PatientId,
    string ProviderId,
    DateTimeOffset StartUtc,
    DateTimeOffset EndUtc,
    string Status,
    string? Notes,
    string ETag);

public sealed record StudyEntity(
    Guid StudyId,
    string PatientId,
    Guid? EncounterId,
    string StudyType,
    string Status,
    string? OrderedBy,
    string? PerformedBy,
    DateTimeOffset DateTimePerformed,
    string? NarrativeJson,
    string? StructuredFindingsJson,
    string ETag);

public sealed record AttachmentEntity(
    Guid AttachmentId,
    Guid StudyId,
    string Kind,
    string ContentType,
    long SizeBytes,
    string BlobName,
    string Sha256,
    string UploadState);
