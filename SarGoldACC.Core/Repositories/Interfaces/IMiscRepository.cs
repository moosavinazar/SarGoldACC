using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IMiscRepository
{
    Task<Misc> GetByIdAsync(long id);
    Task<List<Misc>> GetAllAsync();
    Task<Misc> AddAsync(Misc misc);
    Task UpdateAsync(Misc misc);
    Task DeleteAsync(Misc misc);
}