using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class BankRepository : IBankRepository
{
    private readonly AppDbContext _context;

    public BankRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Bank> GetByIdAsync(long id)
    {
        return await _context.Banks.FindAsync(id);
    }

    public async Task<List<Bank>> GetAllAsync()
    {
        return await _context.Banks.ToListAsync();
    }

    public async Task UpdateAsync(Bank bank)
    {
        _context.Banks.Update(bank);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Bank bank)
    {
        _context.Banks.Remove(bank);
        await _context.SaveChangesAsync();
    }
}