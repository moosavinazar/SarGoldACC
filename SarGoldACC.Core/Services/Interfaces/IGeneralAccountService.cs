using SarGoldACC.Core.DTOs.GeneralAccountDto;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IGeneralAccountService
{
    Task<GeneralAccountDto> GetByIdAsync(long id);
    Task<List<GeneralAccountDto>> GetAllAsync();
}