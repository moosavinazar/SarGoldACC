using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.DTOs.Document;
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
    private readonly IDocumentService _documentService;

    public CustomerService(
        ICustomerRepository customerRepository, 
        IMapper mapper, 
        AppDbContext appDbContext,
        IAuthorizationService authorizationService,
        ICounterpartyService counterpartyService,
        IDocumentService documentService)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _dbContext = appDbContext;
        _authorizationService = authorizationService;
        _counterpartyService = counterpartyService;
        _documentService = documentService;
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
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        if (customerCreate.BranchId == 0)
        {
            customerCreate.BranchId = _authorizationService.GetCurrentUser().BranchId;
        }
        try
        {
            var customer = _mapper.Map<Customer>(customerCreate);
            var addedCustomer = await _customerRepository.AddAsync(customer);
            var counterparty = new CounterpartyDto
            {
                CustomerId = addedCustomer.Id,
                BranchId = customerCreate.BranchId
            };
            var addedCounterparty = await _counterpartyService.AddAsync(counterparty);
            CounterpartyDto counterpartyDto = _mapper.Map<CounterpartyDto>(addedCounterparty.Data);
            var counterpartyOpeningEntry = new CounterPartyOpeningEntryDto
            {
                counterpartyId = counterpartyDto.Id,
                branchId = counterpartyDto.BranchId,
                WeightBed = customerCreate.WeightBed ?? 0,
                WeightBes = customerCreate.WeightBes ?? 0,
                RiyalBed = customerCreate.RiyalBed ?? 0,
                RiyalBes = customerCreate.RiyalBes ?? 0
            };
            await _documentService.AddCounterpartyOpeningEntry(counterpartyOpeningEntry);
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

    public async Task<ResultDto> UpdateAsync(CustomerUpdateDto customerUpdate)
    {
        var customer = await _customerRepository.GetByIdAsync(customerUpdate.Id);
        if (customer == null)
            throw new Exception("Customer not found");

        _mapper.Map(customerUpdate, customer);
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