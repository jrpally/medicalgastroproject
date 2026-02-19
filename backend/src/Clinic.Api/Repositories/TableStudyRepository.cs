using Clinic.Api.Interfaces;
using Clinic.Api.Models;

namespace Clinic.Api.Repositories;

public sealed class TableStudyRepository : IStudyRepository
{
    public Task UpsertAsync(StudyEntity entity, CancellationToken ct)
    {
        // TODO: persist to Studies table by patient partition.
        return Task.CompletedTask;
    }

    public Task<StudyEntity?> GetAsync(Guid studyId, CancellationToken ct)
    {
        // TODO: read via study lookup table.
        return Task.FromResult<StudyEntity?>(null);
    }

    public Task<IReadOnlyCollection<StudyEntity>> QueryByPatientAsync(string patientId, string? type, DateTimeOffset? from, DateTimeOffset? to, CancellationToken ct)
    {
        return Task.FromResult<IReadOnlyCollection<StudyEntity>>(Array.Empty<StudyEntity>());
    }
}
