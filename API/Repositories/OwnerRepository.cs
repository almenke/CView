using Microsoft.EntityFrameworkCore;
using CView.API.Data;
using CView.API.Models;

namespace CView.API.Repositories;

public class OwnerRepository : Repository<Owner>, IOwnerRepository
{
    public OwnerRepository(CViewDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Owner>> GetOwnersByProjectIdAsync(int projectId)
    {
        return await _dbSet
            .Where(o => o.ProjectId == projectId)
            .OrderBy(o => o.Name)
            .ToListAsync();
    }

    public async Task DeleteOwnersByProjectIdAsync(int projectId)
    {
        var owners = await _dbSet.Where(o => o.ProjectId == projectId).ToListAsync();
        _dbSet.RemoveRange(owners);
        await _context.SaveChangesAsync();
    }
}
