using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Customer;

namespace SarGoldACC.Core.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> GetByIdAsync(long id);
    Task<List<CustomerDto>> GetAllAsync();
    Task<ResultDto> AddAsync(CustomerCreateDto customerCreate);
    Task<ResultDto> UpdateAsync(CustomerUpdateDto customerUpdate);
    Task DeleteAsync(long id);
}