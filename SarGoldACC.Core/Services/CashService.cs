using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Cash;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class CashService : ICashService
{
    private readonly ICashRepository _cashRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICounterpartyService _counterpartyService;
    private readonly IDocumentService _documentService;

    public CashService(
        ICashRepository cashRepository, 
        IMapper mapper, 
        AppDbContext appDbContext,
        IAuthorizationService authorizationService,
        ICounterpartyService counterpartyService,
        IDocumentService documentService)
    {
        _cashRepository = cashRepository;
        _mapper = mapper;
        _dbContext = appDbContext;
        _authorizationService = authorizationService;
        _counterpartyService = counterpartyService;
        _documentService = documentService;
    }

    public async Task<CashDto> GetByIdAsync(long id)
    {
        var cash = await _cashRepository.GetByIdAsync(id);
        return _mapper.Map<CashDto>(cash);
    }

    public async Task<List<CashDto>> GetAllAsync()
    {
        var cashs = await _cashRepository.GetAllAsync();
        return _mapper.Map<List<CashDto>>(cashs);
    }

    public async Task<ResultDto> AddAsync(CashCreateDto cashCreate)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        if (cashCreate.BranchId == 0)
        {
            cashCreate.BranchId = _authorizationService.GetCurrentUser().BranchId;
        }
        try
        {
            var cash = _mapper.Map<Cash>(cashCreate);
            var counterparty = new CounterpartyDto
            {
                Cash = cash,
                BranchId = cashCreate.BranchId
            };
            var entry = _dbContext.Entry(cash);
            var addedCounterparty = await _counterpartyService.AddAsync(counterparty);
            var counterpartyDto = _mapper.Map<CounterpartyDto>(addedCounterparty.Data);
            var counterpartyOpeningEntry = new CounterPartyOpeningEntryDto()
            {
                counterpartyId = counterpartyDto.Id,
                branchId = counterpartyDto.BranchId,
                WeightBed = cashCreate.WeightBed ?? 0,
                WeightBes = cashCreate.WeightBes ?? 0,
                RiyalBed = cashCreate.RiyalBed ?? 0,
                RiyalBes = cashCreate.RiyalBes ?? 0
            };
            await _documentService.AddCounterpartyOpeningEntry(counterpartyOpeningEntry);
            await transaction.CommitAsync();
            return new ResultDto
            {
                Success = true,
                Message = "Cash added.",
                Data = _mapper.Map<CashDto>(cash)
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

    public async Task<ResultDto> UpdateAsync(CashUpdateDto cashUpdate)
    {
        var cash = await _cashRepository.GetByIdAsync(cashUpdate.Id);
        if (cash == null)
            throw new Exception("Cash not found");

        _mapper.Map(cashUpdate, cash);
        await _cashRepository.UpdateAsync(cash);
        return new ResultDto
        {
            Success = true,
            Message = "Cash added.",
            Data = _mapper.Map<CashDto>(cash)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var cash = await _cashRepository.GetByIdAsync(id);
        if (cash == null)
            throw new Exception("Cash not found");

        await _cashRepository.DeleteAsync(cash);
    }
}