using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ICoinCategoryRepository
{
    Task<CoinCategory> GetByIdAsync(long id);
    Task<List<CoinCategory>> GetAllAsync();
    Task AddAsync(CoinCategory coinCategory);
    Task UpdateAsync(CoinCategory coinCategory);
    Task DeleteAsync(CoinCategory coinCategory);
}