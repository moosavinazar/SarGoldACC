using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IMadeRepository
{
    Task<Made> GetByIdAsync(long id);
    Task<List<Made>> GetAllAsync();
    Task<Made> AddAsync(Made made);
    Task UpdateAsync(Made made);
    Task DeleteAsync(Made made);
}