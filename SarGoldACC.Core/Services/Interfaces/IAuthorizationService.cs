namespace SarGoldACC.Core.Services.Interfaces;

public interface IAuthorizationService
{
    void LoadUserPermissionsAsync(long userId);
}