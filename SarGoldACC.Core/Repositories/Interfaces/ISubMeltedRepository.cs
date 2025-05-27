using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ISubMeltedRepository
{
    Task<SubMelted> GetByIdAsync(long id);
    Task<List<SubMelted>> GetAllAsync();
    Task<List<SubMelted>> GetAllWithDetailAsync();
    Task<SubMelted> AddAsync(SubMelted subMelted);
    Task UpdateAsync(SubMelted subMelted);
    Task DeleteAsync(SubMelted subMelted);
}