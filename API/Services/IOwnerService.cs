using CView.API.DTOs;

namespace CView.API.Services;

public interface IOwnerService
{
    Task<IEnumerable<OwnerDto>> GetOwnersByProjectIdAsync(int projectId);
    Task<OwnerDto?> GetOwnerByIdAsync(int id);
    Task<OwnerDto> CreateOwnerAsync(int projectId, CreateOwnerDto dto);
    Task<OwnerDto?> UpdateOwnerAsync(int id, UpdateOwnerDto dto);
    Task<bool> DeleteOwnerAsync(int id);
}
