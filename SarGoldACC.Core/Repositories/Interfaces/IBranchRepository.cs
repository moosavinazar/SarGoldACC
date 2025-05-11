using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IBranchRepository
{
    Task<Branch> GetByIdAsync(long id);
    Task<List<Branch>> GetAllAsync();
    Task<Branch> AddAsync(Branch branch);
    Branch AddWithoutSave(Branch branch);
    Task UpdateAsync(Branch branch);
    Task DeleteAsync(Branch branch);
}