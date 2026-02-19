using Clinic.Api.DTOs;
using Clinic.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Api.Controllers;

[ApiController]
[Authorize]
[Route("attachments")]
public sealed class AttachmentsController(IAttachmentService attachmentService, IAiService aiService) : ControllerBase
{
    [HttpPost("upload/init")]
    public async Task<IActionResult> Init([FromBody] object request, CancellationToken ct)
        => Ok(await attachmentService.InitializeUploadAsync(request, ct));

    [HttpPut("upload/chunk")]
    public async Task<IActionResult> Chunk([FromBody] object request, CancellationToken ct)
        => Ok(await attachmentService.UploadChunkAsync(request, ct));

    [HttpPost("upload/complete")]
    public async Task<IActionResult> Complete([FromBody] object request, CancellationToken ct)
        => Ok(await attachmentService.CompleteUploadAsync(request, ct));

    [HttpPost("/studies/{studyId:guid}/attachments/{attachmentId:guid}/link")]
    public async Task<IActionResult> Link(Guid studyId, Guid attachmentId, CancellationToken ct)
    {
        await attachmentService.LinkAsync(studyId, attachmentId, ct);
        return Accepted();
    }

    [HttpPost("{id:guid}/analyze")]
    public async Task<IActionResult> Analyze(Guid id, [FromBody] AnalyzeAttachmentRequest request, CancellationToken ct)
    {
        await aiService.EnqueueAnalysisAsync(id, request, ct);
        return Accepted();
    }

    [HttpGet("{id:guid}/analysis")]
    public async Task<IActionResult> GetAnalysis(Guid id, CancellationToken ct)
        => Ok(await aiService.GetAnalysisAsync(id, ct));
}
