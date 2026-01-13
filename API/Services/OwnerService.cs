using CView.API.DTOs;
using CView.API.Models;
using CView.API.Repositories;

namespace CView.API.Services;

public class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;

    public OwnerService(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    public async Task<IEnumerable<OwnerDto>> GetOwnersByProjectIdAsync(int projectId)
    {
        var owners = await _ownerRepository.GetOwnersByProjectIdAsync(projectId);
        return owners.Select(MapToDto);
    }

    public async Task<OwnerDto?> GetOwnerByIdAsync(int id)
    {
        var owner = await _ownerRepository.GetByIdAsync(id);
        return owner == null ? null : MapToDto(owner);
    }

    public async Task<OwnerDto> CreateOwnerAsync(int projectId, CreateOwnerDto dto)
    {
        var owner = new Owner
        {
            ProjectId = projectId,
            Name = dto.Name,
            Title = dto.Title
        };

        await _ownerRepository.AddAsync(owner);
        return MapToDto(owner);
    }

    public async Task<OwnerDto?> UpdateOwnerAsync(int id, UpdateOwnerDto dto)
    {
        var owner = await _ownerRepository.GetByIdAsync(id);
        if (owner == null) return null;

        owner.Name = dto.Name;
        owner.Title = dto.Title;

        await _ownerRepository.UpdateAsync(owner);
        return MapToDto(owner);
    }

    public async Task<bool> DeleteOwnerAsync(int id)
    {
        var owner = await _ownerRepository.GetByIdAsync(id);
        if (owner == null) return false;

        await _ownerRepository.DeleteAsync(owner);
        return true;
    }

    private static OwnerDto MapToDto(Owner owner)
    {
        return new OwnerDto
        {
            Id = owner.Id,
            ProjectId = owner.ProjectId,
            Name = owner.Name,
            Title = owner.Title,
            CreatedAt = owner.CreatedAt,
            UpdatedAt = owner.UpdatedAt
        };
    }
}
