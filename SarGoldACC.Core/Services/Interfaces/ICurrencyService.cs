using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Currency;

namespace SarGoldACC.Core.Services.Interfaces;

public interface ICurrencyService
{
    Task<CurrencyDto> GetByIdAsync(long id);
    Task<List<CurrencyDto>> GetAllAsync();
    Task<ResultDto> AddAsync(CurrencyCreateDto currencyCreate);
    Task<ResultDto> UpdateAsync(CurrencyDto currency);
    Task DeleteAsync(long id);
}