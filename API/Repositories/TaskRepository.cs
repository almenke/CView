using Microsoft.EntityFrameworkCore;
using CView.API.Data;
using CView.API.Models;

namespace CView.API.Repositories;

public class TaskRepository : Repository<ProjectTask>, ITaskRepository
{
    public TaskRepository(CViewDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProjectTask>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _dbSet
            .Include(t => t.Owner)
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.PlannedStartsAt)
            .ToListAsync();
    }

    public async Task<ProjectTask?> GetTaskWithOwnerAsync(int id)
    {
        return await _dbSet
            .Include(t => t.Owner)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task DeleteTasksByProjectIdAsync(int projectId)
    {
        var tasks = await _dbSet.Where(t => t.ProjectId == projectId).ToListAsync();
        _dbSet.RemoveRange(tasks);
        await _context.SaveChangesAsync();
    }
}
