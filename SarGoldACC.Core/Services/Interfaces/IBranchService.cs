using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Branch;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IBranchService
{
    Task<BranchDto> GetByIdAsync(long id);
    Task<List<BranchDto>> GetAllAsync();
    Task<ResultDto> AddAsync(BranchCreateDto branchCreate);
    Task<ResultDto> UpdateAsync(BranchDto branch);
    Task DeleteAsync(long id);
}