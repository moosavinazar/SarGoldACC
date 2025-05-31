using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class CoinCategoryRepository : ICoinCategoryRepository
{
    private readonly AppDbContext _context;

    public CoinCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CoinCategory> GetByIdAsync(long id)
    {
        return await _context.CoinCategories.FindAsync(id);
    }

    public async Task<List<CoinCategory>> GetAllAsync()
    {
        return await _context.CoinCategories.ToListAsync();
    }

    public async Task AddAsync(CoinCategory coinCategory)
    {
        _context.CoinCategories.Add(coinCategory);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CoinCategory coinCategory)
    {
        _context.CoinCategories.Update(coinCategory);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CoinCategory coinCategory)
    {
        _context.CoinCategories.Remove(coinCategory);
        await _context.SaveChangesAsync();
    }
    
}