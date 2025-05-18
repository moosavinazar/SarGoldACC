using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Cost;

namespace SarGoldACC.Core.Services.Interfaces;

public interface ICostService
{
    Task<CostDto> GetByIdAsync(long id);
    Task<List<CostDto>> GetAllAsync();
    Task<ResultDto> AddAsync(CostCreateDto costCreate);
    Task<ResultDto> UpdateAsync(CostUpdateDto cost);
    Task DeleteAsync(long id);
}