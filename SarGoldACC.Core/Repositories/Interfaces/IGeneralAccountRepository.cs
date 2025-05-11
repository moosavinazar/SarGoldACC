using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IGeneralAccountRepository
{
    Task<GeneralAccount> GetByIdAsync(long id);
    Task<List<GeneralAccount>> GetAllAsync();
}