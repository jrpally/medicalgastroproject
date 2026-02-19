using Clinic.Api.Interfaces;
using Clinic.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicies.ClinicalStaff)]
[Route("slots")]
public sealed class SlotsController(IAppointmentService appointmentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string providerId, [FromQuery] DateTimeOffset from, [FromQuery] DateTimeOffset to, CancellationToken ct)
    {
        var result = await appointmentService.GetSlotsAsync(providerId, from, to, ct);
        return Ok(result);
    }
}
