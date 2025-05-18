using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class CostRepository : ICostRepository
{
    private readonly AppDbContext _context;

    public CostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cost> GetByIdAsync(long id)
    {
        return await _context.Costs.FindAsync(id);
    }

    public async Task<List<Cost>> GetAllAsync()
    {
        return await _context.Costs.ToListAsync();
    }

    public async Task UpdateAsync(Cost cost)
    {
        _context.Costs.Update(cost);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Cost cost)
    {
        _context.Costs.Remove(cost);
        await _context.SaveChangesAsync();
    }
}