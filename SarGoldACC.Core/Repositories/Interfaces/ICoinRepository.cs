using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ICoinRepository
{
    Task<Coin> GetByIdAsync(long id);
    Task<List<Coin>> GetAllAsync();
    Task<List<Coin>> GetAllWithDetailAsync();
    Task<Coin> AddAsync(Coin coin);
    Task UpdateAsync(Coin coin);
    Task DeleteAsync(Coin coin);
}