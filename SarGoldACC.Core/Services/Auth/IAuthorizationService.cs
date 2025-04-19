namespace SarGoldACC.Core.Services.Auth;

public interface IAuthorizationService
{
    Task LoadUserPermissionsAsync(long userId);
    bool HasPermission(string permissionName);
}