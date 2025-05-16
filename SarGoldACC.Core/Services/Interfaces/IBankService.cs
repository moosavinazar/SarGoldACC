using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Bank;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IBankService
{
    Task<BankDto> GetByIdAsync(long id);
    Task<List<BankDto>> GetAllAsync();
    Task<ResultDto> AddAsync(BankCreateDto bankCreate);
    Task<ResultDto> UpdateAsync(BankUpdateDto bank);
    Task DeleteAsync(long id);
}