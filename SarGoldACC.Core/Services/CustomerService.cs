using AutoMapper;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICounterpartyService _counterpartyService;

    public CustomerService(
        ICustomerRepository customerRepository, 
        IMapper mapper, 
        AppDbContext appDbContext,
        IAuthorizationService authorizationService,
        ICounterpartyService counterpartyService)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _dbContext = appDbContext;
        _authorizationService = authorizationService;
        _counterpartyService = counterpartyService;
    }

    public async Task<CustomerDto> GetByIdAsync(long id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return _mapper.Map<List<CustomerDto>>(customers);
    }

    public async Task<ResultDto> AddAsync(CustomerCreateDto customerCreate)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        if (customerCreate.BranchId == null)
        {
            customerCreate.BranchId = _authorizationService.GetCurrentUserBranchId();
        }
        try
        {
            var customer = _mapper.Map<Customer>(customerCreate);
            var addedCustomer = _customerRepository.AddWithoutSave(customer);
            await _dbContext.SaveChangesAsync();
            var counterparty = new CounterpartyDto
            {
                CustomerId = addedCustomer.Id,
                BranchId = customerCreate.BranchId
            };
            _counterpartyService.AddWithoutSave(counterparty);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return new ResultDto
            {
                Success = true,
                Message = "Customer added.",
                Data = _mapper.Map<CustomerDto>(customer)
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ResultDto
            {
                Success = false,
                Message = ex.Message,
            };
        }
    }

    public async Task<ResultDto> UpdateAsync(CustomerDto customerDto)
    {
        var customer = await _customerRepository.GetByIdAsync(customerDto.Id);
        if (customer == null)
            throw new Exception("Customer not found");

        _mapper.Map(customerDto, customer);
        await _customerRepository.UpdateAsync(customer);
        return new ResultDto
        {
            Success = true,
            Message = "Customer added.",
            Data = _mapper.Map<CustomerDto>(customer)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new Exception("Customer not found");

        await _customerRepository.DeleteAsync(customer);
    }
}