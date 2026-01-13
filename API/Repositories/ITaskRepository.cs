using CView.API.Models;

namespace CView.API.Repositories;

public interface ITaskRepository : IRepository<ProjectTask>
{
    Task<IEnumerable<ProjectTask>> GetTasksByProjectIdAsync(int projectId);
    Task<ProjectTask?> GetTaskWithOwnerAsync(int id);
    Task DeleteTasksByProjectIdAsync(int projectId);
}
