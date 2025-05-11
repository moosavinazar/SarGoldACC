using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ICityRepository
{
    Task<City> GetByIdAsync(long id);
    Task<List<City>> GetAllAsync();
    Task AddAsync(City city);
    Task UpdateAsync(City city);
    Task DeleteAsync(City city);
}