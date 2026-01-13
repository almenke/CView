using CView.API.DTOs;
using CView.API.Models;
using CView.API.Repositories;

namespace CView.API.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IOwnerRepository _ownerRepository;

    public ProjectService(IProjectRepository projectRepository, ISprintRepository sprintRepository, IOwnerRepository ownerRepository)
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
        _ownerRepository = ownerRepository;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
    {
        var projects = await _projectRepository.GetAllProjectsWithDetailsAsync();
        return projects.Select(MapToDto);
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        var project = await _projectRepository.GetProjectWithDetailsAsync(id);
        return project == null ? null : MapToDto(project);
    }

    public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            OwnerId = dto.OwnerId,
            StartsAt = dto.StartsAt,
            EndsAt = dto.EndsAt
        };

        await _projectRepository.AddAsync(project);

        // Auto-generate sprints
        await GenerateSprintsForProject(project);

        var result = await _projectRepository.GetProjectWithDetailsAsync(project.Id);
        return MapToDto(result!);
    }

    public async Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) return null;

        project.Name = dto.Name;
        project.OwnerId = dto.OwnerId;
        project.StartsAt = dto.StartsAt;
        project.EndsAt = dto.EndsAt;

        await _projectRepository.UpdateAsync(project);

        var result = await _projectRepository.GetProjectWithDetailsAsync(project.Id);
        return MapToDto(result!);
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) return false;

        // Delete owners first (NoAction FK constraint requires manual cleanup)
        await _ownerRepository.DeleteOwnersByProjectIdAsync(id);

        await _projectRepository.DeleteAsync(project);
        return true;
    }

    public async Task RegenerateSprintsAsync(int projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        if (project == null) return;

        // Delete existing sprints
        await _sprintRepository.DeleteSprintsByProjectIdAsync(projectId);

        // Generate new sprints
        await GenerateSprintsForProject(project);
    }

    private async Task GenerateSprintsForProject(Project project)
    {
        var sprintDuration = TimeSpan.FromDays(14); // 2-week sprints
        var currentStart = project.StartsAt;
        var sprintNumber = 1;

        while (currentStart < project.EndsAt)
        {
            var sprintEnd = currentStart.Add(sprintDuration).AddDays(-1);
            if (sprintEnd > project.EndsAt) sprintEnd = project.EndsAt;

            var sprint = new Sprint
            {
                ProjectId = project.Id,
                Name = $"Sprint {sprintNumber}",
                StartsAt = currentStart,
                EndsAt = sprintEnd
            };

            await _sprintRepository.AddAsync(sprint);

            currentStart = sprintEnd.AddDays(1);
            sprintNumber++;
        }
    }

    private static ProjectDto MapToDto(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            OwnerId = project.OwnerId,
            OwnerName = project.Owner?.Name,
            StartsAt = project.StartsAt,
            EndsAt = project.EndsAt,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Sprints = project.Sprints.Select(s => new SprintDto
            {
                Id = s.Id,
                ProjectId = s.ProjectId,
                Name = s.Name,
                StartsAt = s.StartsAt,
                EndsAt = s.EndsAt,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList(),
            Owners = project.Owners.Select(o => new OwnerDto
            {
                Id = o.Id,
                ProjectId = o.ProjectId,
                Name = o.Name,
                Title = o.Title,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt
            }).ToList(),
            Tasks = project.Tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                ProjectId = t.ProjectId,
                Name = t.Name,
                OwnerId = t.OwnerId,
                OwnerName = t.Owner?.Name,
                PlannedStartsAt = t.PlannedStartsAt,
                PlannedEndsAt = t.PlannedEndsAt,
                ActualStartsAt = t.ActualStartsAt,
                ActualEndsAt = t.ActualEndsAt,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList()
        };
    }
}
