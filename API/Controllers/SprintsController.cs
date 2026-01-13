using Microsoft.AspNetCore.Mvc;
using CView.API.DTOs;
using CView.API.Services;

namespace CView.API.Controllers;

[ApiController]
[Route("api/projects/{projectId}/[controller]")]
public class SprintsController : ControllerBase
{
    private readonly ISprintService _sprintService;

    public SprintsController(ISprintService sprintService)
    {
        _sprintService = sprintService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SprintDto>>> GetByProjectId(int projectId)
    {
        var sprints = await _sprintService.GetSprintsByProjectIdAsync(projectId);
        return Ok(sprints);
    }

    [HttpGet("/api/sprints/{id}")]
    public async Task<ActionResult<SprintDto>> GetById(int id)
    {
        var sprint = await _sprintService.GetSprintByIdAsync(id);
        if (sprint == null) return NotFound();
        return Ok(sprint);
    }

    [HttpPost]
    public async Task<ActionResult<SprintDto>> Create(int projectId, [FromBody] CreateSprintDto dto)
    {
        var sprint = await _sprintService.CreateSprintAsync(projectId, dto);
        return CreatedAtAction(nameof(GetById), new { id = sprint.Id }, sprint);
    }

    [HttpPut("/api/sprints/{id}")]
    public async Task<ActionResult<SprintDto>> Update(int id, [FromBody] UpdateSprintDto dto)
    {
        var sprint = await _sprintService.UpdateSprintAsync(id, dto);
        if (sprint == null) return NotFound();
        return Ok(sprint);
    }

    [HttpDelete("/api/sprints/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _sprintService.DeleteSprintAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
