using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly AppDbContext _context;

    public IncomeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Income> GetByIdAsync(long id)
    {
        return await _context.Incomes.FindAsync(id);
    }

    public async Task<List<Income>> GetAllAsync()
    {
        return await _context.Incomes.ToListAsync();
    }

    public async Task UpdateAsync(Income income)
    {
        _context.Incomes.Update(income);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Income income)
    {
        _context.Incomes.Remove(income);
        await _context.SaveChangesAsync();
    }
}