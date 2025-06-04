using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Setting;

namespace SarGoldACC.Core.Services.Interfaces;

public interface ISettingService
{
    Task<SettingDto> GetByIdAsync(long id);
    Task<SettingDto> GetSetting();
    Task<List<SettingDto>> GetAllAsync();
    Task<ResultDto> AddAsync(SettingCreateDto settingCreate);
    Task<ResultDto> UpdateAsync(SettingDto setting);
    Task DeleteAsync(long id);
}