using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IInvoiceRowRepository
{
    Task<InvoiceRow> GetByIdAsync(long id);
    Task<List<InvoiceRow>> GetAllAsync();
    Task AddAsync(InvoiceRow invoiceRow);
    InvoiceRow AddWithoutSave(InvoiceRow invoiceRow);
    Task UpdateAsync(InvoiceRow invoiceRow);
    Task DeleteAsync(InvoiceRow invoiceRow);
}