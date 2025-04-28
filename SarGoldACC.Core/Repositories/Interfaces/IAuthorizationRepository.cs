namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IAuthorizationRepository
{
    Task<List<string>> LoadUserPermissionsAsync(long userId);
}