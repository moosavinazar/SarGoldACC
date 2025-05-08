using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.City;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class CityService : ICityService
{
    
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public CityService(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<CityDto> GetByIdAsync(long id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        return _mapper.Map<CityDto>(city);
    }

    public async Task<List<CityDto>> GetAllAsync()
    {
        var cities = await _cityRepository.GetAllAsync();
        return _mapper.Map<List<CityDto>>(cities);
    }

    public async Task<ResultDto> AddAsync(CityCreateDto cityCreate)
    {
        try
        {
            var city = new City
            {
                Name = cityCreate.Name,
            };
            await _cityRepository.AddAsync(city);
            return new ResultDto
            {
                Success = true,
                Message = "City added.",
                Data = _mapper.Map<CityDto>(city)
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

    public async Task<ResultDto> UpdateAsync(CityDto cityDto)
    {
        var city = await _cityRepository.GetByIdAsync(cityDto.Id);
        if (city == null)
            throw new Exception("City not found");

        _mapper.Map(cityDto, city);
        await _cityRepository.UpdateAsync(city);
        return new ResultDto
        {
            Success = true,
            Message = "City added.",
            Data = _mapper.Map<CityDto>(city)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var city = await _cityRepository.GetByIdAsync(id);
        if (city == null)
            throw new Exception("City not found");

        await _cityRepository.DeleteAsync(city);
    }
}