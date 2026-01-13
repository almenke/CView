using CView.API.Models;

namespace CView.API.Repositories;

public interface ISprintRepository : IRepository<Sprint>
{
    Task<IEnumerable<Sprint>> GetSprintsByProjectIdAsync(int projectId);
    Task DeleteSprintsByProjectIdAsync(int projectId);
}
