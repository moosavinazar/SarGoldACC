using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models.Auth;

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

        // در دنیای واقعی حتما باید پسورد هش شده باشه و با هش مقایسه بشه
        return await db.Users
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .ThenInclude(gp => gp.GroupPermissions)
            .ThenInclude(g => g.Permission)
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    }
}
