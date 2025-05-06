namespace SarGoldACC.Core.Services.Interfaces;

public interface IAuthorizationService
{
    Task<HashSet<string>> LoadUserPermissionsAsync(long userId);
    string GetCurrentUserIdAsString();
    bool HasPermission(string permissionName);
}