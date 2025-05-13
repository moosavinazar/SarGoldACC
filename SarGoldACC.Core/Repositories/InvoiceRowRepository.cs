using Microsoft.EntityFrameworkCore;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;

namespace SarGoldACC.Core.Repositories;

public class InvoiceRowRepository : IInvoiceRowRepository
{
    private readonly AppDbContext _context;

    public InvoiceRowRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InvoiceRow> GetByIdAsync(long id)
    {
        return await _context.InvoiceRows.FindAsync(id);
    }

    public async Task<List<InvoiceRow>> GetAllAsync()
    {
        return await _context.InvoiceRows.ToListAsync();
    }

    public async Task<InvoiceRow> AddAsync(InvoiceRow invoiceRow)
    {
        _context.InvoiceRows.Add(invoiceRow);
        await _context.SaveChangesAsync();
        return invoiceRow;
    }
    
    public InvoiceRow AddWithoutSave(InvoiceRow invoiceRow)
    {
        var result = _context.InvoiceRows.Add(invoiceRow);
        return result.Entity;
    }

    public async Task UpdateAsync(InvoiceRow invoiceRow)
    {
        _context.InvoiceRows.Update(invoiceRow);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(InvoiceRow invoiceRow)
    {
        _context.InvoiceRows.Remove(invoiceRow);
        await _context.SaveChangesAsync();
    }
}