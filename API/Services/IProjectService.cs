using CView.API.DTOs;

namespace CView.API.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
    Task<ProjectDto?> GetProjectByIdAsync(int id);
    Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto);
    Task<ProjectDto?> UpdateProjectAsync(int id, UpdateProjectDto dto);
    Task<bool> DeleteProjectAsync(int id);
    Task RegenerateSprintsAsync(int projectId);
}
