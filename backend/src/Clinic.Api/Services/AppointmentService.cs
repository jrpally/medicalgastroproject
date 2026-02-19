using Clinic.Api.DTOs;
using Clinic.Api.Interfaces;
using Clinic.Api.Models;

namespace Clinic.Api.Services;

public sealed class AppointmentService(
    IAppointmentRepository repo,
    IMedicalCenterDirectoryService directoryService,
    IGoogleCalendarSyncService calendarSyncService) : IAppointmentService
{
    public async Task<CreateAppointmentResponse> CreateAsync(CreateAppointmentRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.MedicalCenterId)) throw new ArgumentException("medicalCenterId is required");
        if (request.StartUtc >= request.EndUtc) throw new ArgumentException("start must be before end");

        await directoryService.EnsurePersonnelInCenterAsync(request.MedicalCenterId, request.ProviderId, request.SecretaryId, ct);

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

        var providerCalendarId = await directoryService.ResolveCalendarIdAsync(request.MedicalCenterId, request.ProviderId, ct);
        var secretaryCalendarId = await directoryService.ResolveCalendarIdAsync(request.MedicalCenterId, request.SecretaryId, ct);

        var providerEventId = await calendarSyncService.CreateEventAsync(providerCalendarId, request, ct);
        var secretaryEventId = await calendarSyncService.CreateEventAsync(secretaryCalendarId, request, ct);

        return new CreateAppointmentResponse(
            request.AppointmentId,
            request.MedicalCenterId,
            "Booked",
            [
                new CalendarSyncStatus(request.ProviderId, providerCalendarId, providerEventId, "Synced"),
                new CalendarSyncStatus(request.SecretaryId, secretaryCalendarId, secretaryEventId, "Synced")
            ]);
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
