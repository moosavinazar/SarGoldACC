using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Cost;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class CostService : ICostService
{
    private readonly ICostRepository _costRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICounterpartyService _counterpartyService;
    private readonly IDocumentService _documentService;

    public CostService(
        ICostRepository costRepository, 
        IMapper mapper, 
        AppDbContext appDbContext,
        IAuthorizationService authorizationService,
        ICounterpartyService counterpartyService,
        IDocumentService documentService)
    {
        _costRepository = costRepository;
        _mapper = mapper;
        _dbContext = appDbContext;
        _authorizationService = authorizationService;
        _counterpartyService = counterpartyService;
        _documentService = documentService;
    }

    public async Task<CostDto> GetByIdAsync(long id)
    {
        var cost = await _costRepository.GetByIdAsync(id);
        return _mapper.Map<CostDto>(cost);
    }

    public async Task<List<CostDto>> GetAllAsync()
    {
        var costs = await _costRepository.GetAllAsync();
        return _mapper.Map<List<CostDto>>(costs);
    }

    public async Task<ResultDto> AddAsync(CostCreateDto costCreate)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        if (costCreate.BranchId == 0)
        {
            costCreate.BranchId = _authorizationService.GetCurrentUser().BranchId;
        }
        try
        {
            var cost = _mapper.Map<Cost>(costCreate);
            var counterparty = new CounterpartyDto
            {
                Cost = cost,
                BranchId = costCreate.BranchId
            };
            var entry = _dbContext.Entry(cost);
            var addedCounterparty = await _counterpartyService.AddAsync(counterparty);
            var counterpartyDto = _mapper.Map<CounterpartyDto>(addedCounterparty.Data);
            var counterpartyOpeningEntry = new OrderDto()
            {
                counterpartyId = counterpartyDto.Id,
                branchId = counterpartyDto.BranchId,
                WeightBed = costCreate.WeightBed ?? 0,
                WeightBes = costCreate.WeightBes ?? 0,
                RiyalBed = costCreate.RiyalBed ?? 0,
                RiyalBes = costCreate.RiyalBes ?? 0
            };
            await _documentService.AddCounterpartyOpeningEntry(counterpartyOpeningEntry);
            await transaction.CommitAsync();
            return new ResultDto
            {
                Success = true,
                Message = "Cost added.",
                Data = _mapper.Map<CostDto>(cost)
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

    public async Task<ResultDto> UpdateAsync(CostUpdateDto costUpdate)
    {
        var cost = await _costRepository.GetByIdAsync(costUpdate.Id);
        if (cost == null)
            throw new Exception("Cost not found");

        _mapper.Map(costUpdate, cost);
        await _costRepository.UpdateAsync(cost);
        return new ResultDto
        {
            Success = true,
            Message = "Cost added.",
            Data = _mapper.Map<CostDto>(cost)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var cost = await _costRepository.GetByIdAsync(id);
        if (cost == null)
            throw new Exception("Cost not found");

        await _costRepository.DeleteAsync(cost);
    }
}