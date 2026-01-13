using CView.API.Models;

namespace CView.API.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetProjectWithDetailsAsync(int id);
    Task<IEnumerable<Project>> GetAllProjectsWithDetailsAsync();
}
