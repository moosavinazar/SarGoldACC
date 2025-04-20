using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Utils;

namespace SarGoldACC.Core.Services.Auth;

public class AuthenticationService
{
    private readonly IDbContextFactory _dbContextFactory;

    public AuthenticationService(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<User?> AuthenticateUserAsync(string username, string password)
    {
        using var db = _dbContextFactory.CreateDbContext();

        var user = await db.Users
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .ThenInclude(g => g.GroupPermissions)
            .ThenInclude(gp => gp.Permission)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return null;

        return PasswordHasher.VerifyPassword(password, user.Password) ? user : null;
    }
}
