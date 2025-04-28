using AutoMapper;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class AuthorizationService : IAuthorizationService
{
    IAuthorizationRepository _authorizationRepository; 
    private readonly HashSet<string> _permissions = new();

    public AuthorizationService(IAuthorizationRepository authorizationRepository)
    {
        _authorizationRepository = authorizationRepository;
    }

    public void LoadUserPermissionsAsync(long userId)
    {
        _permissions.Clear();

        var permissions = _authorizationRepository.LoadUserPermissionsAsync(userId).Result;
        
        foreach (var p in permissions)
            _permissions.Add(p);
    }

    public bool HasPermission(string permissionName)
    {
        return _permissions.Contains(permissionName);
    }
}