using Clinic.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Authorize]
[Route("patients")]
public sealed class PatientsController(IStudyService studyService) : ControllerBase
{
    [HttpGet("search")]
    public IActionResult Search([FromQuery] string q)
    {
        return Ok(new { Query = q, Items = Array.Empty<object>() });
    }

    [HttpGet("{id}/timeline")]
    public async Task<IActionResult> Timeline(string id, [FromQuery] DateTimeOffset? from, [FromQuery] DateTimeOffset? to, CancellationToken ct)
    {
        var timeline = await studyService.GetTimelineAsync(id, from, to, ct);
        return Ok(timeline);
    }
}
