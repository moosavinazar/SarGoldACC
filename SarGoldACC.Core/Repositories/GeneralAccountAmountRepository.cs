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

    public async Task<OrderAmount> GetByIdAsync(long id)
    {
        return await _context.GeneralAccountAmounts.FindAsync(id);
    }

    public async Task<List<OrderAmount>> GetAllAsync()
    {
        return await _context.GeneralAccountAmounts.ToListAsync();
    }

    public async Task<OrderAmount> AddAsync(OrderAmount orderAmount)
    {
        _context.GeneralAccountAmounts.Add(orderAmount);
        await _context.SaveChangesAsync();
        return orderAmount;
    }

    public async Task UpdateAsync(OrderAmount orderAmount)
    {
        _context.GeneralAccountAmounts.Update(orderAmount);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(OrderAmount orderAmount)
    {
        _context.GeneralAccountAmounts.Remove(orderAmount);
        await _context.SaveChangesAsync();
    }
}