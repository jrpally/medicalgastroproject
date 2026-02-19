using Clinic.Api.DTOs;
using Clinic.Api.Interfaces;
using Clinic.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Authorize(Policy = AuthorizationPolicies.DoctorOrAdministrator)]
[Route("reports")]
public sealed class ReportsController(IReportService reportService) : ControllerBase
{
    [HttpPost("render-docx")]
    public async Task<IActionResult> Render([FromBody] RenderReportRequest request, CancellationToken ct)
    {
        var response = await reportService.RenderAsync(request, ct);
        return Ok(response);
    }

    [HttpPost("{id:guid}/finalize")]
    public async Task<IActionResult> Finalize(Guid id, CancellationToken ct)
    {
        await reportService.FinalizeAsync(id, Request.Headers.IfMatch.ToString(), ct);
        return Accepted();
    }
}
