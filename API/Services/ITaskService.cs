using CView.API.DTOs;

namespace CView.API.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(int projectId);
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<TaskDto> CreateTaskAsync(int projectId, CreateTaskDto dto);
    Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteTaskAsync(int id);
}
