using CView.API.DTOs;
using CView.API.Models;
using CView.API.Repositories;

namespace CView.API.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(int projectId)
    {
        var tasks = await _taskRepository.GetTasksByProjectIdAsync(projectId);
        return tasks.Select(MapToDto);
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetTaskWithOwnerAsync(id);
        return task == null ? null : MapToDto(task);
    }

    public async Task<TaskDto> CreateTaskAsync(int projectId, CreateTaskDto dto)
    {
        var task = new ProjectTask
        {
            ProjectId = projectId,
            Name = dto.Name,
            OwnerId = dto.OwnerId,
            PlannedStartsAt = dto.PlannedStartsAt,
            PlannedEndsAt = dto.PlannedEndsAt,
            ActualStartsAt = dto.ActualStartsAt,
            ActualEndsAt = dto.ActualEndsAt,
            Status = dto.Status
        };

        await _taskRepository.AddAsync(task);

        var result = await _taskRepository.GetTaskWithOwnerAsync(task.Id);
        return MapToDto(result!);
    }

    public async Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) return null;

        task.Name = dto.Name;
        task.OwnerId = dto.OwnerId;
        task.PlannedStartsAt = dto.PlannedStartsAt;
        task.PlannedEndsAt = dto.PlannedEndsAt;
        task.ActualStartsAt = dto.ActualStartsAt;
        task.ActualEndsAt = dto.ActualEndsAt;
        task.Status = dto.Status;

        await _taskRepository.UpdateAsync(task);

        var result = await _taskRepository.GetTaskWithOwnerAsync(task.Id);
        return MapToDto(result!);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) return false;

        await _taskRepository.DeleteAsync(task);
        return true;
    }

    private static TaskDto MapToDto(ProjectTask task)
    {
        return new TaskDto
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            Name = task.Name,
            OwnerId = task.OwnerId,
            OwnerName = task.Owner?.Name,
            PlannedStartsAt = task.PlannedStartsAt,
            PlannedEndsAt = task.PlannedEndsAt,
            ActualStartsAt = task.ActualStartsAt,
            ActualEndsAt = task.ActualEndsAt,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
