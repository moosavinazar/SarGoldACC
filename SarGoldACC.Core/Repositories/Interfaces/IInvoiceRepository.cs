using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice> GetByIdAsync(long id);
    Task<List<Invoice>> GetAllAsync();
    Task<Invoice> AddAsync(Invoice invoice);
    Invoice AddWithoutSave(Invoice invoice);
    Task UpdateAsync(Invoice invoice);
    Task DeleteAsync(Invoice invoice);
}