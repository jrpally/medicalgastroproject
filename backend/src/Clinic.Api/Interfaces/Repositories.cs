using Clinic.Api.Models;

namespace Clinic.Api.Interfaces;

public interface IAppointmentRepository
{
    Task<bool> ExistsOverlapAsync(string providerId, DateTimeOffset startUtc, DateTimeOffset endUtc, CancellationToken ct);
    Task UpsertAsync(AppointmentEntity entity, CancellationToken ct);
}

public interface IStudyRepository
{
    Task UpsertAsync(StudyEntity entity, CancellationToken ct);
    Task<StudyEntity?> GetAsync(Guid studyId, CancellationToken ct);
    Task<IReadOnlyCollection<StudyEntity>> QueryByPatientAsync(string patientId, string? type, DateTimeOffset? from, DateTimeOffset? to, CancellationToken ct);
}
