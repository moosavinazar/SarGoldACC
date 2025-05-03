using AutoMapper;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class AuthorizationService : IAuthorizationService
{
    IAuthorizationRepository _authorizationRepository; 
    IUserService _userService;
    private readonly HashSet<string> _permissions = new();
    private UserDto _currentUser { get; set; }

    public AuthorizationService(IAuthorizationRepository authorizationRepository, IUserService userService)
    {
        _authorizationRepository = authorizationRepository;
        _userService = userService;
    }

    public async Task<HashSet<string>> LoadUserPermissionsAsync(long userId)
    {
        _permissions.Clear();
        
        _currentUser = await _userService.GetByIdAsync(userId);
        var permissions = await _authorizationRepository.LoadUserPermissionsAsync(userId);
        
        foreach (var p in permissions)
            _permissions.Add(p);

        return await Task.FromResult(_permissions);
    }

    public bool HasPermission(string permissionName)
    {
        return _permissions.Contains(permissionName);
    }

    public string GetCurrentUserIdAsString()
    {
        return _currentUser?.Id.ToString() ?? "Unknown";
    }
}