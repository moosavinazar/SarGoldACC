using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;

namespace SarGoldACC.Core.Services.Interfaces;

public interface ICounterpartyService
{
    Task<CounterpartyDto> GetByIdAsync(long id);
    Task<CounterpartyDto> GetByIdAndBranchIdAsync(long id, long branchId);
    Task<List<CounterpartyDto>> GetAllAsync();
    Task<ResultDto> AddAsync(CounterpartyDto counterparty);
    CounterpartyDto AddWithoutSave(CounterpartyDto counterparty);
}