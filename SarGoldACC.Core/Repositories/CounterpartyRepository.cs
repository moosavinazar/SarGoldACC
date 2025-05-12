using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class CounterpartyRepository : ICounterpartyRepository
{
    private readonly AppDbContext _context;

    public CounterpartyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Counterparty> GetByIdAsync(long id)
    {
        return await _context.Counterparties.FindAsync(id);
    }

    public async Task<Counterparty> GetByIdAndBranchIdAsync(long id, long branchId)
    {
        return await _context.Counterparties
            .FirstOrDefaultAsync(c => c.Id == id && c.BranchId == branchId);
    }


    public async Task<List<Counterparty>> GetAllAsync()
    {
        return await _context.Counterparties.ToListAsync();
    }

    public async Task AddAsync(Counterparty counterparty)
    {
        _context.Counterparties.Add(counterparty);
        await _context.SaveChangesAsync();
    }
    
    public Counterparty AddWithoutSave(Counterparty counterparty)
    {
        var result = _context.Counterparties.Add(counterparty);
        return result.Entity;
    }
}