using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ICashRepository
{
    Task<Cash> GetByIdAsync(long id);
    Task<List<Cash>> GetAllAsync();
    Task UpdateAsync(Cash cash);
    Task DeleteAsync(Cash cash);
}