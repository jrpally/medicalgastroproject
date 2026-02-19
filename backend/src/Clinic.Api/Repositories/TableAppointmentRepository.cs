using Clinic.Api.Interfaces;
using Clinic.Api.Models;

namespace Clinic.Api.Repositories;

public sealed class TableAppointmentRepository : IAppointmentRepository
{
    public Task<bool> ExistsOverlapAsync(string providerId, DateTimeOffset startUtc, DateTimeOffset endUtc, CancellationToken ct)
    {
        // TODO: query provider/day partition and detect overlaps atomically.
        return Task.FromResult(false);
    }

    public Task UpsertAsync(AppointmentEntity entity, CancellationToken ct)
    {
        // TODO: persist entity in Appointments + AppointmentsByPatient index tables.
        return Task.CompletedTask;
    }
}
