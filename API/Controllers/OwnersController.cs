using Microsoft.AspNetCore.Mvc;
using CView.API.DTOs;
using CView.API.Services;

namespace CView.API.Controllers;

[ApiController]
[Route("api/projects/{projectId}/[controller]")]
public class OwnersController : ControllerBase
{
    private readonly IOwnerService _ownerService;

    public OwnersController(IOwnerService ownerService)
    {
        _ownerService = ownerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OwnerDto>>> GetByProjectId(int projectId)
    {
        var owners = await _ownerService.GetOwnersByProjectIdAsync(projectId);
        return Ok(owners);
    }

    [HttpGet("/api/owners/{id}")]
    public async Task<ActionResult<OwnerDto>> GetById(int id)
    {
        var owner = await _ownerService.GetOwnerByIdAsync(id);
        if (owner == null) return NotFound();
        return Ok(owner);
    }

    [HttpPost]
    public async Task<ActionResult<OwnerDto>> Create(int projectId, [FromBody] CreateOwnerDto dto)
    {
        var owner = await _ownerService.CreateOwnerAsync(projectId, dto);
        return CreatedAtAction(nameof(GetById), new { id = owner.Id }, owner);
    }

    [HttpPut("/api/owners/{id}")]
    public async Task<ActionResult<OwnerDto>> Update(int id, [FromBody] UpdateOwnerDto dto)
    {
        var owner = await _ownerService.UpdateOwnerAsync(id, dto);
        if (owner == null) return NotFound();
        return Ok(owner);
    }

    [HttpDelete("/api/owners/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _ownerService.DeleteOwnerAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
