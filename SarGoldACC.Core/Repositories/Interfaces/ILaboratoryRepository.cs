using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ILaboratoryRepository
{
    Task<Laboratory> GetByIdAsync(long id);
    Task<List<Laboratory>> GetAllAsync();
    Task UpdateAsync(Laboratory laboratory);
    Task DeleteAsync(Laboratory laboratory);
}