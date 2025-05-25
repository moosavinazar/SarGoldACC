using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class OrderAmountRepository : IOrderAmountRepository
{
    private readonly AppDbContext _context;

    public OrderAmountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderAmount> GetByIdAsync(long id)
    {
        return await _context.OrderAmounts.FindAsync(id);
    }

    public async Task<List<OrderAmount>> GetAllAsync()
    {
        return await _context.OrderAmounts.ToListAsync();
    }

    public async Task<OrderAmount> AddAsync(OrderAmount orderAmount)
    {
        _context.OrderAmounts.Add(orderAmount);
        await _context.SaveChangesAsync();
        return orderAmount;
    }

    public async Task UpdateAsync(OrderAmount orderAmount)
    {
        _context.OrderAmounts.Update(orderAmount);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(OrderAmount orderAmount)
    {
        _context.OrderAmounts.Remove(orderAmount);
        await _context.SaveChangesAsync();
    }
}