using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IGeneralAccountAmountRepository
{
    Task<GeneralAccountAmount> GetByIdAsync(long id);
    Task<List<GeneralAccountAmount>> GetAllAsync();
    Task<GeneralAccountAmount> AddAsync(GeneralAccountAmount generalAccountAmount);
    Task UpdateAsync(GeneralAccountAmount generalAccountAmount);
    Task DeleteAsync(GeneralAccountAmount generalAccountAmount);
}