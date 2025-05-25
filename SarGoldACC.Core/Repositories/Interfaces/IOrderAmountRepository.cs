using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IOrderAmountRepository
{
    Task<OrderAmount> GetByIdAsync(long id);
    Task<List<OrderAmount>> GetAllAsync();
    Task<OrderAmount> AddAsync(OrderAmount orderAmount);
    Task UpdateAsync(OrderAmount orderAmount);
    Task DeleteAsync(OrderAmount orderAmount);
}