using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Repositories.Interfaces;

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
        return await _context.Users
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .ThenInclude(g => g.GroupPermissions)
            .ThenInclude(gp => gp.Permission)
            .FirstOrDefaultAsync(u => u.Username == username);
    }
}