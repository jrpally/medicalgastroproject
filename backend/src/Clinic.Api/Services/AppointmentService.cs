using Clinic.Api.DTOs;
using Clinic.Api.Interfaces;
using Clinic.Api.Models;

namespace Clinic.Api.Services;

public sealed class AppointmentService(IAppointmentRepository repo) : IAppointmentService
{
    public async Task<AppointmentEntity> CreateAsync(CreateAppointmentRequest request, CancellationToken ct)
    {
        if (request.StartUtc >= request.EndUtc) throw new ArgumentException("start must be before end");
        if (await repo.ExistsOverlapAsync(request.ProviderId, request.StartUtc, request.EndUtc, ct))
        {
            throw new InvalidOperationException("APPOINTMENT_CONFLICT");
        }

        var entity = new AppointmentEntity(
            request.AppointmentId,
            request.PatientId,
            request.ProviderId,
            request.StartUtc,
            request.EndUtc,
            "Booked",
            request.Notes,
            ETag: "*");

        await repo.UpsertAsync(entity, ct);
        return entity;
    }

    public Task TransitionAsync(Guid appointmentId, string targetStatus, CancellationToken ct)
    {
        // TODO: enforce finite-state workflow and publish SignalR events.
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<SlotDto>> GetSlotsAsync(string providerId, DateTimeOffset from, DateTimeOffset to, CancellationToken ct)
    {
        return Task.FromResult<IReadOnlyCollection<SlotDto>>(Array.Empty<SlotDto>());
    }
}
