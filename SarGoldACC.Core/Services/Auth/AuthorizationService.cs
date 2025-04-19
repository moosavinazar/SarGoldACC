using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;

namespace SarGoldACC.Core.Services.Auth;

public class AuthorizationService : IAuthorizationService
{
    private readonly AppDbContext _context;
    private readonly HashSet<string> _permissions = new();

    public AuthorizationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task LoadUserPermissionsAsync(long userId)
    {
        _permissions.Clear();

        var permissions = await _context.UserGroups
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.GroupPermissions)
            .Select(gp => gp.Permission.Name)
            .Distinct()
            .ToListAsync();


        foreach (var p in permissions)
            _permissions.Add(p);
    }

    public bool HasPermission(string permissionName)
    {
        return _permissions.Contains(permissionName);
    }
}