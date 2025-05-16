using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly AppDbContext _context;

    public CurrencyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Currency> GetByIdAsync(long id)
    {
        return await _context.Currencies.FindAsync(id);
    }

    public async Task<List<Currency>> GetAllAsync()
    {
        return await _context.Currencies.ToListAsync();
    }

    public async Task AddAsync(Currency currency)
    {
        _context.Currencies.Add(currency);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Currency currency)
    {
        _context.Currencies.Update(currency);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Currency currency)
    {
        _context.Currencies.Remove(currency);
        await _context.SaveChangesAsync();
    }
}