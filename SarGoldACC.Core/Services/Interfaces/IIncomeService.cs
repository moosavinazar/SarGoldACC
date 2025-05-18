using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Income;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IIncomeService
{
    Task<IncomeDto> GetByIdAsync(long id);
    Task<List<IncomeDto>> GetAllAsync();
    Task<ResultDto> AddAsync(IncomeCreateDto incomeCreate);
    Task<ResultDto> UpdateAsync(IncomeUpdateDto income);
    Task DeleteAsync(long id);
}