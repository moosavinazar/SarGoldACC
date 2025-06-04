using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ISettingRepository
{
    Task<Setting> GetByIdAsync(long id);
    Task<List<Setting>> GetAllAsync();
    Task AddAsync(Setting setting);
    Task UpdateAsync(Setting setting);
    Task DeleteAsync(Setting setting);
}