using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IIncomeRepository
{
    Task<Income> GetByIdAsync(long id);
    Task<List<Income>> GetAllAsync();
    Task UpdateAsync(Income income);
    Task DeleteAsync(Income income);
}