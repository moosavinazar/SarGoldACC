using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class SubMeltedRepository : ISubMeltedRepository
{
    private readonly AppDbContext _context;

    public SubMeltedRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SubMelted> GetByIdAsync(long id)
    {
        return await _context.SubMelteds.FindAsync(id);
    }

    public async Task<List<SubMelted>> GetAllAsync()
    {
        return await _context.SubMelteds.ToListAsync();
    }
    
    public async Task<List<SubMelted>> GetAllWithDetailAsync()
    {
        return await _context.SubMelteds
            .Include(s => s.Melted)
            .ThenInclude(m => m.Laboratory)
            .Include(s => s.Box)
            .ToListAsync();
    }


    public async Task<SubMelted> AddAsync(SubMelted subMelted)
    {
        _context.SubMelteds.Add(subMelted);
        await _context.SaveChangesAsync();
        return subMelted;
    }

    public async Task UpdateAsync(SubMelted subMelted)
    {
        _context.SubMelteds.Update(subMelted);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(SubMelted subMelted)
    {
        _context.SubMelteds.Remove(subMelted);
        await _context.SaveChangesAsync();
    }
}