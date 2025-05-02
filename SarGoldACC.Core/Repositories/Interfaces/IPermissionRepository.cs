using SarGoldACC.Core.Models.Auth;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IPermissionRepository
{
    Task<Permission> GetByIdAsync(long id);
    Task<List<Permission>> GetAllAsync();
}