using Clinic.Api.DTOs;
using Clinic.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Authorize]
[Route("studies")]
public sealed class StudiesController(IStudyService studyService, IAiService aiService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudyRequest request, CancellationToken ct)
    {
        var study = await studyService.CreateAsync(request, ct);
        return Created($"/studies/{study.StudyId}", study);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] object patch, CancellationToken ct)
    {
        await studyService.PatchAsync(id, patch, Request.Headers.IfMatch.ToString(), ct);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct) => Ok(await studyService.GetAsync(id, ct));

    [HttpGet]
    public async Task<IActionResult> Query([FromQuery] string patientId, [FromQuery] string? type, [FromQuery] DateTimeOffset? from, [FromQuery] DateTimeOffset? to, CancellationToken ct)
        => Ok(await studyService.QueryAsync(patientId, type, from, to, ct));

    [HttpPost("{id:guid}/ai/draft-report-json")]
    public async Task<IActionResult> Draft(Guid id, CancellationToken ct)
        => Ok(await aiService.DraftReportJsonAsync(id, ct));
}
