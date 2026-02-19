using Clinic.Api.DTOs;
using Clinic.Api.Interfaces;

namespace Clinic.Api.Services;

public sealed class GoogleCalendarSyncService : IGoogleCalendarSyncService
{
    public Task<string> CreateEventAsync(string calendarId, CreateAppointmentRequest request, CancellationToken ct)
    {
        // TODO: Replace with Google Calendar API integration using OAuth delegated access
        // scoped to each staff account.
        var eventId = $"gcal-{request.AppointmentId:N}-{Math.Abs(calendarId.GetHashCode())}";
        return Task.FromResult(eventId);
    }
}
