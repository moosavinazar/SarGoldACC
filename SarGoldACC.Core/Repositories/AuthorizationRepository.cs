using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class AuthorizationRepository : IAuthorizationRepository
{
    private readonly AppDbContext _context;

    public AuthorizationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> LoadUserPermissionsAsync(long userId)
    {
        return await _context.UserGroups
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.GroupPermissions)
            .Select(gp => gp.Permission.Name)
            .Distinct()
            .ToListAsync();
    }

}