using SarGoldACC.Core.Models.Auth;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IAuthenticationRepository
{
    Task<User> AuthenticateUserAsync(string username, string password);
}