using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class CashRepository : ICashRepository
{
    private readonly AppDbContext _context;

    public CashRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cash> GetByIdAsync(long id)
    {
        return await _context.Cash.FindAsync(id);
    }

    public async Task<List<Cash>> GetAllAsync()
    {
        return await _context.Cash.ToListAsync();
    }

    public async Task UpdateAsync(Cash cash)
    {
        _context.Cash.Update(cash);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Cash cash)
    {
        _context.Cash.Remove(cash);
        await _context.SaveChangesAsync();
    }
}