using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class GeneralAccountAmountRepository : IGeneralAccountAmountRepository
{
    private readonly AppDbContext _context;

    public GeneralAccountAmountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GeneralAccountAmount> GetByIdAsync(long id)
    {
        return await _context.GeneralAccountAmounts.FindAsync(id);
    }

    public async Task<List<GeneralAccountAmount>> GetAllAsync()
    {
        return await _context.GeneralAccountAmounts.ToListAsync();
    }

    public async Task<GeneralAccountAmount> AddAsync(GeneralAccountAmount generalAccountAmount)
    {
        _context.GeneralAccountAmounts.Add(generalAccountAmount);
        await _context.SaveChangesAsync();
        return generalAccountAmount;
    }

    public async Task UpdateAsync(GeneralAccountAmount generalAccountAmount)
    {
        _context.GeneralAccountAmounts.Update(generalAccountAmount);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(GeneralAccountAmount generalAccountAmount)
    {
        _context.GeneralAccountAmounts.Remove(generalAccountAmount);
        await _context.SaveChangesAsync();
    }
}