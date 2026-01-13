using CView.API.Models;

namespace CView.API.DTOs;

public class TaskDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OwnerId { get; set; }
    public string? OwnerName { get; set; }
    public DateTime PlannedStartsAt { get; set; }
    public DateTime PlannedEndsAt { get; set; }
    public DateTime? ActualStartsAt { get; set; }
    public DateTime? ActualEndsAt { get; set; }
    public StatusEnum Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateTaskDto
{
    public string Name { get; set; } = string.Empty;
    public int? OwnerId { get; set; }
    public DateTime PlannedStartsAt { get; set; }
    public DateTime PlannedEndsAt { get; set; }
    public DateTime? ActualStartsAt { get; set; }
    public DateTime? ActualEndsAt { get; set; }
    public StatusEnum Status { get; set; } = StatusEnum.NotSet;
}

public class UpdateTaskDto
{
    public string Name { get; set; } = string.Empty;
    public int? OwnerId { get; set; }
    public DateTime PlannedStartsAt { get; set; }
    public DateTime PlannedEndsAt { get; set; }
    public DateTime? ActualStartsAt { get; set; }
    public DateTime? ActualEndsAt { get; set; }
    public StatusEnum Status { get; set; }
}
