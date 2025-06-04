using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Setting;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class SettingService : ISettingService
{
    
    private readonly ISettingRepository _settingRepository;
    private readonly IMapper _mapper;

    public SettingService(ISettingRepository settingRepository, IMapper mapper)
    {
        _settingRepository = settingRepository;
        _mapper = mapper;
    }

    public async Task<SettingDto> GetByIdAsync(long id)
    {
        var setting = await _settingRepository.GetByIdAsync(id);
        return _mapper.Map<SettingDto>(setting);
    }
    
    public async Task<SettingDto> GetSetting()
    {
        var setting = await _settingRepository.GetByIdAsync(1);
        return _mapper.Map<SettingDto>(setting);
    }

    public async Task<List<SettingDto>> GetAllAsync()
    {
        var cities = await _settingRepository.GetAllAsync();
        return _mapper.Map<List<SettingDto>>(cities);
    }

    public async Task<ResultDto> AddAsync(SettingCreateDto settingCreate)
    {
        try
        {
            var setting = new Setting()
            {
                CustomerImageUrl = settingCreate.CustomerImageUrl,
            };
            await _settingRepository.AddAsync(setting);
            return new ResultDto
            {
                Success = true,
                Message = "Setting added.",
                Data = _mapper.Map<SettingDto>(setting)
            };
        }
        catch (Exception ex)
        {
            return new ResultDto
            {
                Success = false,
                Message = ex.Message,
            };
        }
    }

    public async Task<ResultDto> UpdateAsync(SettingDto settingDto)
    {
        var setting = await _settingRepository.GetByIdAsync(settingDto.Id);
        if (setting == null)
            throw new Exception("Setting not found");

        _mapper.Map(settingDto, setting);
        await _settingRepository.UpdateAsync(setting);
        return new ResultDto
        {
            Success = true,
            Message = "Setting added.",
            Data = _mapper.Map<SettingDto>(setting)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var setting = await _settingRepository.GetByIdAsync(id);
        if (setting == null)
            throw new Exception("Setting not found");

        await _settingRepository.DeleteAsync(setting);
    }
}