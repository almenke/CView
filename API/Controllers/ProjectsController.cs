using Microsoft.AspNetCore.Mvc;
using CView.API.DTOs;
using CView.API.Services;

namespace CView.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IExcelImportService _excelImportService;

    public ProjectsController(IProjectService projectService, IExcelImportService excelImportService)
    {
        _projectService = projectService;
        _excelImportService = excelImportService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto dto)
    {
        var project = await _projectService.CreateProjectAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> Update(int id, [FromBody] UpdateProjectDto dto)
    {
        var project = await _projectService.UpdateProjectAsync(id, dto);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/regenerate-sprints")]
    public async Task<IActionResult> RegenerateSprints(int id)
    {
        await _projectService.RegenerateSprintsAsync(id);
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpPost("{id}/import")]
    public async Task<ActionResult<ImportResultDto>> Import(int id, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        using var stream = file.OpenReadStream();
        var result = await _excelImportService.ImportExcelAsync(id, stream);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
