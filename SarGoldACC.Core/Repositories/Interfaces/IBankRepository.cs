using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IBankRepository
{
    Task<Bank> GetByIdAsync(long id);
    Task<List<Bank>> GetAllAsync();
    Task UpdateAsync(Bank bank);
    Task DeleteAsync(Bank bank);
}