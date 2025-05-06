using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.DTOs.Auth.Group;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IGroupService
{
    Task<GroupDto> GetByIdAsync(long id);
    Task<List<GroupDto>> GetAllAsync();
    Task<ResultDto> AddAsync(GroupCreateDto createGroupDto);
    Task<ResultDto> UpdateAsync(GroupDto groupDto);
    Task DeleteAsync(long id);
}