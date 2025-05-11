using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class GeneralAccountRepository : IGeneralAccountRepository
{
    private readonly AppDbContext _context;

    public GeneralAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GeneralAccount> GetByIdAsync(long id)
    {
        return await _context.GeneralAccounts.FindAsync(id);
    }

    public async Task<List<GeneralAccount>> GetAllAsync()
    {
        return await _context.GeneralAccounts.ToListAsync();
    }
}