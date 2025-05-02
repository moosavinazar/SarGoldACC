
using SarGoldACC.Core.Models.Auth;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IGroupRepository
{
    Task<Group> GetByIdAsync(long id);
    Task<List<Group>> GetAllAsync();
    Task AddAsync(Group group);
    Task UpdateAsync(Group group);
    Task DeleteAsync(Group group);
}