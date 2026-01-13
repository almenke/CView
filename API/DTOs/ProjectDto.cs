using CView.API.Models;

namespace CView.API.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OwnerId { get; set; }
    public string? OwnerName { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<SprintDto> Sprints { get; set; } = new();
    public List<OwnerDto> Owners { get; set; } = new();
    public List<TaskDto> Tasks { get; set; } = new();
}

public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public int? OwnerId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}

public class UpdateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public int? OwnerId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}
