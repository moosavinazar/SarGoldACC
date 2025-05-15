using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(long id);
    Task<List<Customer>> GetAllAsync();
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
}