using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class PosRepository : IPosRepository
{
    private readonly AppDbContext _context;

    public PosRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pos> GetByIdAsync(long id)
    {
        return await _context.Poses.FindAsync(id);
    }

    public async Task<List<Pos>> GetAllAsync()
    {
        return await _context.Poses.ToListAsync();
    }

    public async Task UpdateAsync(Pos pos)
    {
        _context.Poses.Update(pos);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Pos pos)
    {
        _context.Poses.Remove(pos);
        await _context.SaveChangesAsync();
    }
}