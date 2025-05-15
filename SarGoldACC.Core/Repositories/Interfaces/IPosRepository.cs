using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IPosRepository
{
    Task<Pos> GetByIdAsync(long id);
    Task<List<Pos>> GetAllAsync();
    Task UpdateAsync(Pos pos);
    Task DeleteAsync(Pos pos);
}