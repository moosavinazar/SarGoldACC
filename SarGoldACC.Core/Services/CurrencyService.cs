using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Currency;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IMapper _mapper;

    public CurrencyService(ICurrencyRepository currencyRepository, IMapper mapper)
    {
        _currencyRepository = currencyRepository;
        _mapper = mapper;
    }

    public async Task<CurrencyDto> GetByIdAsync(long id)
    {
        var currency = await _currencyRepository.GetByIdAsync(id);
        return _mapper.Map<CurrencyDto>(currency);
    }

    public async Task<List<CurrencyDto>> GetAllAsync()
    {
        var currencies = await _currencyRepository.GetAllAsync();
        return _mapper.Map<List<CurrencyDto>>(currencies);
    }

    public async Task<ResultDto> AddAsync(CurrencyCreateDto currencyCreate)
    {
        try
        {
            var currency = new Currency
            {
                Name = currencyCreate.Name,
                Label = currencyCreate.Label,
            };
            await _currencyRepository.AddAsync(currency);
            return new ResultDto
            {
                Success = true,
                Message = "Currency added.",
                Data = _mapper.Map<CurrencyDto>(currency)
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

    public async Task<ResultDto> UpdateAsync(CurrencyDto currencyDto)
    {
        var currency = await _currencyRepository.GetByIdAsync(currencyDto.Id);
        if (currency == null)
            throw new Exception("Currency not found");

        _mapper.Map(currencyDto, currency);
        await _currencyRepository.UpdateAsync(currency);
        return new ResultDto
        {
            Success = true,
            Message = "City added.",
            Data = _mapper.Map<CurrencyDto>(currency)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var currency = await _currencyRepository.GetByIdAsync(id);
        if (currency == null)
            throw new Exception("Currency not found");

        await _currencyRepository.DeleteAsync(currency);
    }
}