using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly AppDbContext _context;

    public GroupRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Group> GetByIdAsync(long id)
    {
        var result = await _context.Groups
            .Include(g => g.GroupPermissions)
            .FirstOrDefaultAsync(g => g.Id == id);
        return result;
    }

    public async Task<List<Group>> GetAllAsync()
    {
        return await _context.Groups.ToListAsync();
    }

    public async Task AddAsync(Group group)
    {
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Group group)
    {
        _context.Groups.Update(group);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Group group)
    {
        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();
    }
}