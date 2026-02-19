using Clinic.Api.DTOs;
using Clinic.Api.Interfaces;
using Clinic.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Authorize]
[Route("appointments")]
public sealed class AppointmentsController(IAppointmentService appointmentService) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.SecretaryOrAdministrator)]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request, CancellationToken ct)
    {
        var created = await appointmentService.CreateAsync(request, ct);
        return Created($"/appointments/{created.AppointmentId}", created);
    }

    [HttpPost("{id:guid}/arrive")]
    [Authorize(Policy = AuthorizationPolicies.ClinicalStaff)]
    public Task<IActionResult> Arrive(Guid id, CancellationToken ct) => Transition(id, "Arrived", ct);

    [HttpPost("{id:guid}/ready")]
    [Authorize(Policy = AuthorizationPolicies.ClinicalStaff)]
    public Task<IActionResult> Ready(Guid id, CancellationToken ct) => Transition(id, "Ready", ct);

    [HttpPost("{id:guid}/start")]
    [Authorize(Policy = AuthorizationPolicies.DoctorOrAdministrator)]
    public Task<IActionResult> Start(Guid id, CancellationToken ct) => Transition(id, "Started", ct);

    [HttpPost("{id:guid}/complete")]
    [Authorize(Policy = AuthorizationPolicies.DoctorOrAdministrator)]
    public Task<IActionResult> Complete(Guid id, CancellationToken ct) => Transition(id, "Completed", ct);

    private async Task<IActionResult> Transition(Guid id, string targetStatus, CancellationToken ct)
    {
        await appointmentService.TransitionAsync(id, targetStatus, ct);
        return Accepted();
    }
}
