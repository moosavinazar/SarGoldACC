using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ICounterpartyRepository
{
    Task<Counterparty> GetByIdAsync(long id);
    Task<List<Counterparty>> GetAllAsync();
    Counterparty AddWithoutSave(Counterparty counterparty);
    Task AddAsync(Counterparty counterparty);
}