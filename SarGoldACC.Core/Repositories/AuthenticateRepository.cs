using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Utils;

namespace SarGoldACC.Core.Repositories;

public class AuthenticateRepository : IAuthenticationRepository
{
    private readonly AppDbContext _context;

    public AuthenticateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        var user = await _context.Users
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .ThenInclude(g => g.GroupPermissions)
            .ThenInclude(gp => gp.Permission)
            .FirstOrDefaultAsync(u => u.Username == username);
        
        if (user == null)
            return null;

        var isValid = PasswordHasher.VerifyPassword(password, user.Password);

        return isValid ? user : null;
    }
}