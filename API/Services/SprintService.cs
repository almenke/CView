using CView.API.DTOs;
using CView.API.Models;
using CView.API.Repositories;

namespace CView.API.Services;

public class SprintService : ISprintService
{
    private readonly ISprintRepository _sprintRepository;

    public SprintService(ISprintRepository sprintRepository)
    {
        _sprintRepository = sprintRepository;
    }

    public async Task<IEnumerable<SprintDto>> GetSprintsByProjectIdAsync(int projectId)
    {
        var sprints = await _sprintRepository.GetSprintsByProjectIdAsync(projectId);
        return sprints.Select(MapToDto);
    }

    public async Task<SprintDto?> GetSprintByIdAsync(int id)
    {
        var sprint = await _sprintRepository.GetByIdAsync(id);
        return sprint == null ? null : MapToDto(sprint);
    }

    public async Task<SprintDto> CreateSprintAsync(int projectId, CreateSprintDto dto)
    {
        var sprint = new Sprint
        {
            ProjectId = projectId,
            Name = dto.Name,
            StartsAt = dto.StartsAt,
            EndsAt = dto.EndsAt
        };

        await _sprintRepository.AddAsync(sprint);
        return MapToDto(sprint);
    }

    public async Task<SprintDto?> UpdateSprintAsync(int id, UpdateSprintDto dto)
    {
        var sprint = await _sprintRepository.GetByIdAsync(id);
        if (sprint == null) return null;

        sprint.Name = dto.Name;
        sprint.StartsAt = dto.StartsAt;
        sprint.EndsAt = dto.EndsAt;

        await _sprintRepository.UpdateAsync(sprint);
        return MapToDto(sprint);
    }

    public async Task<bool> DeleteSprintAsync(int id)
    {
        var sprint = await _sprintRepository.GetByIdAsync(id);
        if (sprint == null) return false;

        await _sprintRepository.DeleteAsync(sprint);
        return true;
    }

    private static SprintDto MapToDto(Sprint sprint)
    {
        return new SprintDto
        {
            Id = sprint.Id,
            ProjectId = sprint.ProjectId,
            Name = sprint.Name,
            StartsAt = sprint.StartsAt,
            EndsAt = sprint.EndsAt,
            CreatedAt = sprint.CreatedAt,
            UpdatedAt = sprint.UpdatedAt
        };
    }
}
