using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Clinic.Api.Hubs;

[Authorize]
public sealed class ClinicEventsHub : Hub
{
    public Task JoinProviderQueue(string providerId) => Groups.AddToGroupAsync(Context.ConnectionId, $"provider:{providerId}");

    public Task JoinPatientTimeline(string patientId) => Groups.AddToGroupAsync(Context.ConnectionId, $"patient:{patientId}");
}
