using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class MadeRepository : IMadeRepository
{
    private readonly AppDbContext _context;

    public MadeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Made> GetByIdAsync(long id)
    {
        return await _context.Mades.FindAsync(id);
    }

    public async Task<List<Made>> GetAllAsync()
    {
        return await _context.Mades.ToListAsync();
    }

    public async Task<Made> AddAsync(Made made)
    {
        _context.Mades.Add(made);
        await _context.SaveChangesAsync();
        return made;
    }

    public async Task UpdateAsync(Made made)
    {
        _context.Mades.Update(made);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Made made)
    {
        _context.Mades.Remove(made);
        await _context.SaveChangesAsync();
    }
}