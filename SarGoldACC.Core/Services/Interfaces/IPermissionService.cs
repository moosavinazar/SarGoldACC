using SarGoldACC.Core.DTOs.Auth;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IPermissionService
{
    Task<PermissionDto> GetByIdAsync(long id);
    Task<List<PermissionDto>> GetAllAsync();
}