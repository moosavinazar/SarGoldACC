using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IBoxRepository
{
    Task<Box> GetByIdAsync(long id);
    Task<List<Box>> GetAllAsync();
    Task AddAsync(Box box);
    Task UpdateAsync(Box box);
    Task DeleteAsync(Box box);
}