using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Bank;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class BankService : IBankService
{
    private readonly IBankRepository _bankRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICounterpartyService _counterpartyService;
    private readonly IDocumentService _documentService;

    public BankService(
        IBankRepository bankRepository, 
        IMapper mapper, 
        AppDbContext appDbContext,
        IAuthorizationService authorizationService,
        ICounterpartyService counterpartyService,
        IDocumentService documentService)
    {
        _bankRepository = bankRepository;
        _mapper = mapper;
        _dbContext = appDbContext;
        _authorizationService = authorizationService;
        _counterpartyService = counterpartyService;
        _documentService = documentService;
    }

    public async Task<BankDto> GetByIdAsync(long id)
    {
        var bank = await _bankRepository.GetByIdAsync(id);
        return _mapper.Map<BankDto>(bank);
    }

    public async Task<List<BankDto>> GetAllAsync()
    {
        var banks = await _bankRepository.GetAllAsync();
        return _mapper.Map<List<BankDto>>(banks);
    }

    public async Task<ResultDto> AddAsync(BankCreateDto bankCreate)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        if (bankCreate.BranchId == 0)
        {
            bankCreate.BranchId = _authorizationService.GetCurrentUser().BranchId;
        }
        try
        {
            var bank = _mapper.Map<Bank>(bankCreate);
            Console.WriteLine("Adding bank");
            Console.WriteLine(bank.Id);
            var counterparty = new CounterpartyDto()
            {
                Bank = bank,
                BranchId = bankCreate.BranchId
            };
            Console.WriteLine(counterparty.Bank.Id);
            Console.WriteLine(counterparty.Id);
            var entry = _dbContext.Entry(bank);
            Console.WriteLine(entry.State); // باید Detached باشد
            var addedCounterparty = await _counterpartyService.AddAsync(counterparty);
            Console.WriteLine(addedCounterparty.Success);
            var counterpartyDto = _mapper.Map<CounterpartyDto>(addedCounterparty.Data);
            Console.WriteLine(counterpartyDto);
            Console.WriteLine(counterpartyDto.Id);
            var counterpartyOpeningEntry = new CounterPartyOpeningEntryDto()
            {
                counterpartyId = counterpartyDto.Id,
                branchId = counterpartyDto.BranchId,
                WeightBed = bankCreate.WeightBed ?? 0,
                WeightBes = bankCreate.WeightBes ?? 0,
                RiyalBed = bankCreate.RiyalBed ?? 0,
                RiyalBes = bankCreate.RiyalBes ?? 0
            };
            await _documentService.AddCounterpartyOpeningEntry(counterpartyOpeningEntry);
            await transaction.CommitAsync();
            return new ResultDto
            {
                Success = true,
                Message = "Bank added.",
                Data = _mapper.Map<BankDto>(bank)
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

    public async Task<ResultDto> UpdateAsync(BankDto bankUpdate)
    {
        var bank = await _bankRepository.GetByIdAsync(bankUpdate.Id);
        if (bank == null)
            throw new Exception("Bank not found");

        _mapper.Map(bankUpdate, bank);
        await _bankRepository.UpdateAsync(bank);
        return new ResultDto
        {
            Success = true,
            Message = "Bank added.",
            Data = _mapper.Map<BankDto>(bank)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var bank = await _bankRepository.GetByIdAsync(id);
        if (bank == null)
            throw new Exception("Bank not found");

        await _bankRepository.DeleteAsync(bank);
    }
}