using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Income;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class IncomeService : IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICounterpartyService _counterpartyService;
    private readonly IDocumentService _documentService;

    public IncomeService(
        IIncomeRepository incomeRepository, 
        IMapper mapper, 
        AppDbContext appDbContext,
        IAuthorizationService authorizationService,
        ICounterpartyService counterpartyService,
        IDocumentService documentService)
    {
        _incomeRepository = incomeRepository;
        _mapper = mapper;
        _dbContext = appDbContext;
        _authorizationService = authorizationService;
        _counterpartyService = counterpartyService;
        _documentService = documentService;
    }

    public async Task<IncomeDto> GetByIdAsync(long id)
    {
        var income = await _incomeRepository.GetByIdAsync(id);
        return _mapper.Map<IncomeDto>(income);
    }

    public async Task<List<IncomeDto>> GetAllAsync()
    {
        var incomes = await _incomeRepository.GetAllAsync();
        return _mapper.Map<List<IncomeDto>>(incomes);
    }

    public async Task<ResultDto> AddAsync(IncomeCreateDto incomeCreate)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        if (incomeCreate.BranchId == 0)
        {
            incomeCreate.BranchId = _authorizationService.GetCurrentUser().BranchId;
        }
        try
        {
            var income = _mapper.Map<Income>(incomeCreate);
            var counterparty = new CounterpartyDto
            {
                Income = income,
                BranchId = incomeCreate.BranchId
            };
            var entry = _dbContext.Entry(income);
            var addedCounterparty = await _counterpartyService.AddAsync(counterparty);
            var counterpartyDto = _mapper.Map<CounterpartyDto>(addedCounterparty.Data);
            var counterpartyOpeningEntry = new OrderDto()
            {
                counterpartyId = counterpartyDto.Id,
                branchId = counterpartyDto.BranchId,
                WeightBed = incomeCreate.WeightBed ?? 0,
                WeightBes = incomeCreate.WeightBes ?? 0,
                RiyalBed = incomeCreate.RiyalBed ?? 0,
                RiyalBes = incomeCreate.RiyalBes ?? 0
            };
            await _documentService.AddCounterpartyOpeningEntry(counterpartyOpeningEntry);
            await transaction.CommitAsync();
            return new ResultDto
            {
                Success = true,
                Message = "Income added.",
                Data = _mapper.Map<IncomeDto>(income)
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(ex);
            return new ResultDto
            {
                Success = false,
                Message = ex.Message,
            };
        }
    }

    public async Task<ResultDto> UpdateAsync(IncomeUpdateDto incomeUpdate)
    {
        var income = await _incomeRepository.GetByIdAsync(incomeUpdate.Id);
        if (income == null)
            throw new Exception("Income not found");

        _mapper.Map(incomeUpdate, income);
        await _incomeRepository.UpdateAsync(income);
        return new ResultDto
        {
            Success = true,
            Message = "Income added.",
            Data = _mapper.Map<IncomeDto>(income)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var income = await _incomeRepository.GetByIdAsync(id);
        if (income == null)
            throw new Exception("Income not found");

        await _incomeRepository.DeleteAsync(income);
    }
}