using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class CoinRepository : ICoinRepository
{
    private readonly AppDbContext _context;

    public CoinRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Coin> GetByIdAsync(long id)
    {
        return await _context.Coins.FindAsync(id);
    }

    public async Task<List<Coin>> GetAllAsync()
    {
        return await _context.Coins.ToListAsync();
    }
    
    public async Task<List<Coin>> GetAllWithDetailAsync()
    {
        return await _context.Coins
            .Include(c => c.CoinCategory)
            .Include(c => c.Box)
            .ToListAsync();
    }

    public async Task<Coin> AddAsync(Coin made)
    {
        _context.Coins.Add(made);
        await _context.SaveChangesAsync();
        return made;
    }

    public async Task UpdateAsync(Coin made)
    {
        _context.Coins.Update(made);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Coin made)
    {
        _context.Coins.Remove(made);
        await _context.SaveChangesAsync();
    }
}