using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Cash;

namespace SarGoldACC.Core.Services.Interfaces;

public interface ICashService
{
    Task<CashDto> GetByIdAsync(long id);
    Task<List<CashDto>> GetAllAsync();
    Task<ResultDto> AddAsync(CashCreateDto cashCreate);
    Task<ResultDto> UpdateAsync(CashUpdateDto cash);
    Task DeleteAsync(long id);
}