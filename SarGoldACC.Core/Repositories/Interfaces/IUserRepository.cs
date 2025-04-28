using SarGoldACC.Core.Models.Auth;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(long id);
    Task<List<User>> GetAllAsync();
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}