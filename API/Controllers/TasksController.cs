using Microsoft.AspNetCore.Mvc;
using CView.API.DTOs;
using CView.API.Services;

namespace CView.API.Controllers;

[ApiController]
[Route("api/projects/{projectId}/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetByProjectId(int projectId)
    {
        var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
        return Ok(tasks);
    }

    [HttpGet("/api/tasks/{id}")]
    public async Task<ActionResult<TaskDto>> GetById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create(int projectId, [FromBody] CreateTaskDto dto)
    {
        var task = await _taskService.CreateTaskAsync(projectId, dto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("/api/tasks/{id}")]
    public async Task<ActionResult<TaskDto>> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var task = await _taskService.UpdateTaskAsync(id, dto);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpDelete("/api/tasks/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _taskService.DeleteTaskAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
