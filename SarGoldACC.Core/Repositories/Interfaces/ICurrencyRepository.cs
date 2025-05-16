using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ICurrencyRepository
{
    Task<Currency> GetByIdAsync(long id);
    Task<List<Currency>> GetAllAsync();
    Task AddAsync(Currency currency);
    Task UpdateAsync(Currency currency);
    Task DeleteAsync(Currency currency);
}