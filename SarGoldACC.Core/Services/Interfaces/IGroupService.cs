using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.DTOs.Auth.Group;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IGroupService
{
    Task<GroupDto> GetByIdAsync(int id);
    Task<List<GroupDto>> GetAllAsync();
    Task AddAsync(GroupCreateDto createGroupDto);
    Task UpdateAsync(GroupDto groupDto);
    Task DeleteAsync(int id);
}