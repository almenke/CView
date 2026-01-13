using CView.API.Models;

namespace CView.API.Repositories;

public interface IOwnerRepository : IRepository<Owner>
{
    Task<IEnumerable<Owner>> GetOwnersByProjectIdAsync(int projectId);
    Task DeleteOwnersByProjectIdAsync(int projectId);
}
