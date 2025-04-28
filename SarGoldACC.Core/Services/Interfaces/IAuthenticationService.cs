using SarGoldACC.Core.DTOs.Auth.User;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IAuthenticationService
{
    Task<UserDto> AuthenticateUserAsync(string username, string password);
}