using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IMadeSubCategoryRepository
{
    Task<MadeSubCategory> GetByIdAsync(long id);
    Task<List<MadeSubCategory>> GetAllAsync();
    Task AddAsync(MadeSubCategory madeSubCategory);
    Task UpdateAsync(MadeSubCategory madeSubCategory);
    Task DeleteAsync(MadeSubCategory madeSubCategory);
}