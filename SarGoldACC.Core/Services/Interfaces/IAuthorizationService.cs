using SarGoldACC.Core.DTOs.Auth.User;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IAuthorizationService
{
    Task<HashSet<string>> LoadUserPermissionsAsync(long userId);
    string GetCurrentUserIdAsString();
    bool HasPermission(string permissionName);
    UserDto GetCurrentUser();
    long GetCurrentUserBranchId();
}