using CView.API.DTOs;

namespace CView.API.Services;

public interface ISprintService
{
    Task<IEnumerable<SprintDto>> GetSprintsByProjectIdAsync(int projectId);
    Task<SprintDto?> GetSprintByIdAsync(int id);
    Task<SprintDto> CreateSprintAsync(int projectId, CreateSprintDto dto);
    Task<SprintDto?> UpdateSprintAsync(int id, UpdateSprintDto dto);
    Task<bool> DeleteSprintAsync(int id);
}
