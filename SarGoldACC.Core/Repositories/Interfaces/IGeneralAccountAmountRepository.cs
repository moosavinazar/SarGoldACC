using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Repositories.Interfaces;

public interface IGeneralAccountAmountRepository
{
    Task<OrderAmount> GetByIdAsync(long id);
    Task<List<OrderAmount>> GetAllAsync();
    Task<OrderAmount> AddAsync(OrderAmount orderAmount);
    Task UpdateAsync(OrderAmount orderAmount);
    Task DeleteAsync(OrderAmount orderAmount);
}