using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.DTOs.Laboratory;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class LaboratoryService : ILaboratoryService
{
    private readonly ILaboratoryRepository _laboratoryRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICounterpartyService _counterpartyService;
    private readonly IDocumentService _documentService;

    public LaboratoryService(
        ILaboratoryRepository laboratoryRepository, 
        IMapper mapper, 
        AppDbContext appDbContext,
        IAuthorizationService authorizationService,
        ICounterpartyService counterpartyService,
        IDocumentService documentService)
    {
        _laboratoryRepository = laboratoryRepository;
        _mapper = mapper;
        _dbContext = appDbContext;
        _authorizationService = authorizationService;
        _counterpartyService = counterpartyService;
        _documentService = documentService;
    }

    public async Task<LaboratoryDto> GetByIdAsync(long id)
    {
        var laboratory = await _laboratoryRepository.GetByIdAsync(id);
        return _mapper.Map<LaboratoryDto>(laboratory);
    }

    public async Task<List<LaboratoryDto>> GetAllAsync()
    {
        var laboratorys = await _laboratoryRepository.GetAllAsync();
        return _mapper.Map<List<LaboratoryDto>>(laboratorys);
    }

    public async Task<ResultDto> AddAsync(LaboratoryCreateDto laboratoryCreate)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        if (laboratoryCreate.BranchId == 0)
        {
            laboratoryCreate.BranchId = _authorizationService.GetCurrentUser().BranchId;
        }
        try
        {
            var laboratory = _mapper.Map<Laboratory>(laboratoryCreate);
            var counterparty = new CounterpartyDto
            {
                Laboratory = laboratory,
                BranchId = laboratoryCreate.BranchId
            };
            var addedCounterparty = await _counterpartyService.AddAsync(counterparty);
            var counterpartyDto = _mapper.Map<CounterpartyDto>(addedCounterparty.Data);
            var counterpartyOpeningEntry = new OrderDto()
            {
                counterpartyId = counterpartyDto.Id,
                branchId = counterpartyDto.BranchId,
                WeightBed = laboratoryCreate.WeightBed ?? 0,
                WeightBes = laboratoryCreate.WeightBes ?? 0,
                RiyalBed = laboratoryCreate.RiyalBed ?? 0,
                RiyalBes = laboratoryCreate.RiyalBes ?? 0
            };
            await _documentService.AddCounterpartyOpeningEntry(counterpartyOpeningEntry);
            await transaction.CommitAsync();
            return new ResultDto
            {
                Success = true,
                Message = "Laboratory added.",
                Data = _mapper.Map<LaboratoryDto>(laboratory)
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

    public async Task<ResultDto> UpdateAsync(LaboratoryUpdateDto laboratoryUpdate)
    {
        var laboratory = await _laboratoryRepository.GetByIdAsync(laboratoryUpdate.Id);
        if (laboratory == null)
            throw new Exception("Laboratory not found");

        _mapper.Map(laboratoryUpdate, laboratory);
        await _laboratoryRepository.UpdateAsync(laboratory);
        return new ResultDto
        {
            Success = true,
            Message = "Laboratory added.",
            Data = _mapper.Map<LaboratoryDto>(laboratory)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var laboratory = await _laboratoryRepository.GetByIdAsync(id);
        if (laboratory == null)
            throw new Exception("Laboratory not found");

        await _laboratoryRepository.DeleteAsync(laboratory);
    }
}