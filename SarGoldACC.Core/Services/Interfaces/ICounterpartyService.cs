using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;

namespace SarGoldACC.Core.Services.Interfaces;

public interface ICounterpartyService
{
    Task<CounterpartyDto> GetByIdAsync(long id);
    Task<List<CounterpartyDto>> GetAllAsync();
    Task<ResultDto> AddAsync(CounterpartyDto counterparty);
    CounterpartyDto AddWithoutSave(CounterpartyDto counterparty);
}