using Microsoft.EntityFrameworkCore;
using CView.API.Data;
using CView.API.Models;

namespace CView.API.Repositories;

public class SprintRepository : Repository<Sprint>, ISprintRepository
{
    public SprintRepository(CViewDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Sprint>> GetSprintsByProjectIdAsync(int projectId)
    {
        return await _dbSet
            .Where(s => s.ProjectId == projectId)
            .OrderBy(s => s.StartsAt)
            .ToListAsync();
    }

    public async Task DeleteSprintsByProjectIdAsync(int projectId)
    {
        var sprints = await _dbSet.Where(s => s.ProjectId == projectId).ToListAsync();
        _dbSet.RemoveRange(sprints);
        await _context.SaveChangesAsync();
    }
}
