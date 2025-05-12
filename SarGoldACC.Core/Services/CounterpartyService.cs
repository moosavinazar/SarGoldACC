using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class CounterpartyService : ICounterpartyService
{
    private readonly ICounterpartyRepository _counterpartyRepository;
    private readonly IMapper _mapper;

    public CounterpartyService(ICounterpartyRepository counterpartyRepository, IMapper mapper)
    {
        _counterpartyRepository = counterpartyRepository;
        _mapper = mapper;
    }

    public async Task<CounterpartyDto> GetByIdAsync(long id)
    {
        var counterparty = await _counterpartyRepository.GetByIdAsync(id);
        return _mapper.Map<CounterpartyDto>(counterparty);
    }

    public async Task<CounterpartyDto> GetByIdAndBranchIdAsync(long id, long branchId)
    {
        var counterparty = await _counterpartyRepository.GetByIdAndBranchIdAsync(id, branchId);
        return _mapper.Map<CounterpartyDto>(counterparty);
    }

    public async Task<List<CounterpartyDto>> GetAllAsync()
    {
        var counterparties = await _counterpartyRepository.GetAllAsync();
        return _mapper.Map<List<CounterpartyDto>>(counterparties);
    }

    public async Task<ResultDto> AddAsync(CounterpartyDto counterpartyDto)
    {
        try
        {
            var counterparty = _mapper.Map<Counterparty>(counterpartyDto);
            await _counterpartyRepository.AddAsync(counterparty);
            return new ResultDto
            {
                Success = true,
                Message = "Counterparty added.",
                Data = _mapper.Map<CounterpartyDto>(counterparty)
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

    public CounterpartyDto AddWithoutSave(CounterpartyDto counterpartyDto)
    {
        var counterparty = _mapper.Map<Counterparty>(counterpartyDto);
        counterparty = _counterpartyRepository.AddWithoutSave(counterparty);
        return _mapper.Map<CounterpartyDto>(counterparty);
    }
}