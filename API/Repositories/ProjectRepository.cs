using Microsoft.EntityFrameworkCore;
using CView.API.Data;
using CView.API.Models;

namespace CView.API.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(CViewDbContext context) : base(context)
    {
    }

    public async Task<Project?> GetProjectWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Owner)
            .Include(p => p.Sprints.OrderBy(s => s.StartsAt))
            .Include(p => p.Owners)
            .Include(p => p.Tasks.OrderBy(t => t.PlannedStartsAt))
                .ThenInclude(t => t.Owner)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetAllProjectsWithDetailsAsync()
    {
        return await _dbSet
            .Include(p => p.Owner)
            .Include(p => p.Sprints.OrderBy(s => s.StartsAt))
            .Include(p => p.Owners)
            .OrderByDescending(p => p.UpdatedAt)
            .ToListAsync();
    }
}
