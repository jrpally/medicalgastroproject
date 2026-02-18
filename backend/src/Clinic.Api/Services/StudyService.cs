using Clinic.Api.DTOs;
using Clinic.Api.Interfaces;
using Clinic.Api.Models;

namespace Clinic.Api.Services;

public sealed class StudyService(IStudyRepository repo) : IStudyService
{
    public async Task<StudyEntity> CreateAsync(CreateStudyRequest request, CancellationToken ct)
    {
        var entity = new StudyEntity(
            request.StudyId,
            request.PatientId,
            request.EncounterId,
            request.StudyType,
            "Draft",
            request.OrderedBy,
            request.PerformedBy,
            request.DateTimePerformed,
            request.NarrativeJson,
            request.StructuredFindingsJson,
            "*");

        await repo.UpsertAsync(entity, ct);
        return entity;
    }

    public Task PatchAsync(Guid studyId, object patch, string etag, CancellationToken ct)
    {
        // TODO: apply JSON Merge/Patch with ETag check.
        return Task.CompletedTask;
    }

    public Task<StudyEntity?> GetAsync(Guid studyId, CancellationToken ct) => repo.GetAsync(studyId, ct);

    public Task<IReadOnlyCollection<StudyEntity>> QueryAsync(string patientId, string? type, DateTimeOffset? from, DateTimeOffset? to, CancellationToken ct)
        => repo.QueryByPatientAsync(patientId, type, from, to, ct);

    public Task<object> GetTimelineAsync(string patientId, DateTimeOffset? from, DateTimeOffset? to, CancellationToken ct)
        => Task.FromResult<object>(new { patientId, from, to, appointments = Array.Empty<object>(), studies = Array.Empty<object>() });
}
