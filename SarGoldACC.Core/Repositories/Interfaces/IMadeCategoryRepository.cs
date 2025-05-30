using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IMadeCategoryRepository
{
    Task<MadeCategory> GetByIdAsync(long id);
    Task<List<MadeCategory>> GetAllAsync();
    Task AddAsync(MadeCategory madeCategory);
    Task UpdateAsync(MadeCategory madeCategory);
    Task DeleteAsync(MadeCategory madeCategory);
}