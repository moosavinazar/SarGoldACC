using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CoinCategory;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class CoinCategoryService : ICoinCategoryService
{
    
    private readonly ICoinCategoryRepository _coinCategoryRepository;
    private readonly IMapper _mapper;

    public CoinCategoryService(ICoinCategoryRepository coinCategoryRepository, IMapper mapper)
    {
        _coinCategoryRepository = coinCategoryRepository;
        _mapper = mapper;
    }

    public async Task<CoinCategoryDto> GetByIdAsync(long id)
    {
        var coinCategory = await _coinCategoryRepository.GetByIdAsync(id);
        return _mapper.Map<CoinCategoryDto>(coinCategory);
    }

    public async Task<List<CoinCategoryDto>> GetAllAsync()
    {
        var cities = await _coinCategoryRepository.GetAllAsync();
        return _mapper.Map<List<CoinCategoryDto>>(cities);
    }

    public async Task<ResultDto> AddAsync(CoinCategoryCreateDto coinCategoryCreate)
    {
        try
        {
            var coinCategory = new CoinCategory()
            {
                Name = coinCategoryCreate.Name,
                Weight = coinCategoryCreate.Weight,
                Weight750 = coinCategoryCreate.Weight750,
                Ayar = coinCategoryCreate.Ayar
            };
            await _coinCategoryRepository.AddAsync(coinCategory);
            return new ResultDto
            {
                Success = true,
                Message = "CoinCategory added.",
                Data = _mapper.Map<CoinCategoryDto>(coinCategory)
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

    public async Task<ResultDto> UpdateAsync(CoinCategoryDto coinCategoryDto)
    {
        var coinCategory = await _coinCategoryRepository.GetByIdAsync(coinCategoryDto.Id);
        if (coinCategory == null)
            throw new Exception("CoinCategory not found");

        _mapper.Map(coinCategoryDto, coinCategory);
        await _coinCategoryRepository.UpdateAsync(coinCategory);
        return new ResultDto
        {
            Success = true,
            Message = "CoinCategory added.",
            Data = _mapper.Map<CoinCategoryDto>(coinCategory)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var coinCategory = await _coinCategoryRepository.GetByIdAsync(id);
        if (coinCategory == null)
            throw new Exception("CoinCategory not found");

        await _coinCategoryRepository.DeleteAsync(coinCategory);
    }
}