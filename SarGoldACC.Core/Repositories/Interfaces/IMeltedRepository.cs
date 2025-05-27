using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IMeltedRepository
{
    Task<Melted> GetByIdAsync(long id);
    Task<List<Melted>> GetAllAsync();
    Task<Melted?> GetByAngAndLaboratoryIdAsync(string ang, long laboratoryId);
    Task<Melted> AddAsync(Melted melted);
    Task UpdateAsync(Melted melted);
    Task DeleteAsync(Melted melted);
}