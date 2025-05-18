using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.DTOs.Pos;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class PosService : IPosService
{
    private readonly IPosRepository _posRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICounterpartyService _counterpartyService;
    private readonly IDocumentService _documentService;
    private readonly IBankService _bankService;

    public PosService(
        IPosRepository posRepository, 
        IMapper mapper, 
        AppDbContext appDbContext,
        IAuthorizationService authorizationService,
        ICounterpartyService counterpartyService,
        IDocumentService documentService,
        IBankService bankService)
    {
        _posRepository = posRepository;
        _mapper = mapper;
        _dbContext = appDbContext;
        _authorizationService = authorizationService;
        _counterpartyService = counterpartyService;
        _documentService = documentService;
        _bankService = bankService;
    }

    public async Task<PosDto> GetByIdAsync(long id)
    {
        var pos = await _posRepository.GetByIdAsync(id);
        return _mapper.Map<PosDto>(pos);
    }

    public async Task<List<PosDto>> GetAllAsync()
    {
        var poss = await _posRepository.GetAllAsync();
        return _mapper.Map<List<PosDto>>(poss);
    }

    public async Task<ResultDto> AddAsync(PosCreateDto posCreate)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        Console.WriteLine(posCreate.BankId);
        var bankDto = await _bankService.GetByIdAsync(posCreate.BankId);
        try
        {
            var pos = _mapper.Map<Pos>(posCreate);
            Console.WriteLine("Adding pos");
            Console.WriteLine(pos.Name);
            Console.WriteLine(bankDto.Counterparty.BranchId);
            var counterparty = new CounterpartyDto()
            {
                Pos = pos,
                BranchId = bankDto.Counterparty.BranchId
            };
            Console.WriteLine(counterparty.Pos.Id);
            Console.WriteLine(counterparty.Id);
            var entry = _dbContext.Entry(pos);
            Console.WriteLine(entry.State); // باید Detached باشد
            var addedCounterparty = await _counterpartyService.AddAsync(counterparty);
            Console.WriteLine(addedCounterparty.Success);
            var counterpartyDto = _mapper.Map<CounterpartyDto>(addedCounterparty.Data);
            Console.WriteLine(counterpartyDto);
            Console.WriteLine(counterpartyDto.Id);
            var counterpartyOpeningEntry = new OrderDto()
            {
                counterpartyId = counterpartyDto.Id,
                branchId = counterpartyDto.BranchId,
                WeightBed = posCreate.WeightBed ?? 0,
                WeightBes = posCreate.WeightBes ?? 0,
                RiyalBed = posCreate.RiyalBed ?? 0,
                RiyalBes = posCreate.RiyalBes ?? 0
            };
            await _documentService.AddCounterpartyOpeningEntry(counterpartyOpeningEntry);
            await transaction.CommitAsync();
            return new ResultDto
            {
                Success = true,
                Message = "Pos added.",
                Data = _mapper.Map<PosDto>(pos)
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

    public async Task<ResultDto> UpdateAsync(PosUpdateDto posUpdate)
    {
        var pos = await _posRepository.GetByIdAsync(posUpdate.Id);
        if (pos == null)
            throw new Exception("Pos not found");

        _mapper.Map(posUpdate, pos);
        await _posRepository.UpdateAsync(pos);
        return new ResultDto
        {
            Success = true,
            Message = "Pos added.",
            Data = _mapper.Map<PosDto>(pos)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var pos = await _posRepository.GetByIdAsync(id);
        if (pos == null)
            throw new Exception("Pos not found");

        await _posRepository.DeleteAsync(pos);
    }
}