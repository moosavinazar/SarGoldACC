using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class MiscRepository : IMiscRepository
{
    private readonly AppDbContext _context;

    public MiscRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Misc> GetByIdAsync(long id)
    {
        return await _context.Miscs.FindAsync(id);
    }

    public async Task<List<Misc>> GetAllAsync()
    {
        return await _context.Miscs.ToListAsync();
    }

    public async Task<Misc> AddAsync(Misc misc)
    {
        _context.Miscs.Add(misc);
        await _context.SaveChangesAsync();
        return misc;
    }

    public async Task UpdateAsync(Misc misc)
    {
        _context.Miscs.Update(misc);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Misc misc)
    {
        _context.Miscs.Remove(misc);
        await _context.SaveChangesAsync();
    }
}