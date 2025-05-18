using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ICostRepository
{
    Task<Cost> GetByIdAsync(long id);
    Task<List<Cost>> GetAllAsync();
    Task UpdateAsync(Cost cost);
    Task DeleteAsync(Cost cost);
}