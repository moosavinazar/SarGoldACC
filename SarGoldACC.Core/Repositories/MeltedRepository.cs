using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class MeltedRepository : IMeltedRepository
{
    private readonly AppDbContext _context;

    public MeltedRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Melted> GetByIdAsync(long id)
    {
        return await _context.Melteds.FindAsync(id);
    }
    
    public async Task<Melted?> GetByAngAndLaboratoryIdAsync(string ang, long laboratoryId)
    {
        return await _context.Melteds
            .FirstOrDefaultAsync(m => m.Ang == ang && m.LaboratoryId == laboratoryId);
    }


    public async Task<List<Melted>> GetAllAsync()
    {
        return await _context.Melteds.ToListAsync();
    }

    public async Task<Melted> AddAsync(Melted melted)
    {
        _context.Melteds.Add(melted);
        await _context.SaveChangesAsync();
        return melted;
    }

    public async Task UpdateAsync(Melted melted)
    {
        _context.Melteds.Update(melted);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Melted melted)
    {
        _context.Melteds.Remove(melted);
        await _context.SaveChangesAsync();
    }
}