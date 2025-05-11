using AutoMapper;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class BranchService : IBranchService
{
    
    private readonly IBranchRepository _branchRepository;
    private readonly IGeneralAccountService _generalAccountService;
    private readonly ICounterpartyService _counterpartyService;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;


    public BranchService(
        IBranchRepository branchRepository, 
        IGeneralAccountService generalAccountService, 
        ICounterpartyService counterpartyService,
        IMapper mapper,
        AppDbContext dbContext)
    {
        _branchRepository = branchRepository;
        _generalAccountService = generalAccountService;
        _counterpartyService = counterpartyService;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<BranchDto> GetByIdAsync(long id)
    {
        var branch = await _branchRepository.GetByIdAsync(id);
        return _mapper.Map<BranchDto>(branch);
    }

    public async Task<List<BranchDto>> GetAllAsync()
    {
        var branches = await _branchRepository.GetAllAsync();
        return _mapper.Map<List<BranchDto>>(branches);
    }

    public async Task<ResultDto> AddAsync(BranchCreateDto branchCreate)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var generalAccountList = await _generalAccountService.GetAllAsync();
            var branch = new Branch
            {
                Name = branchCreate.Name,
            };
            var addedBranch = _branchRepository.AddWithoutSave(branch);
            await _dbContext.SaveChangesAsync();
            foreach (var generalAccount in generalAccountList)
            {
                var counterparty = new CounterpartyDto
                {
                    BranchId = addedBranch.Id,
                    GeneralAccountId = generalAccount.Id,
                };
                _counterpartyService.AddWithoutSave(counterparty);
            }
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            return new ResultDto
            {
                Success = true,
                Message = "Branch added.",
                Data = _mapper.Map<BranchDto>(branch)
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

    public async Task<ResultDto> UpdateAsync(BranchDto branchDto)
    {
        var branch = await _branchRepository.GetByIdAsync(branchDto.Id);
        if (branch == null)
            throw new Exception("Branch not found");

        _mapper.Map(branchDto, branch);
        await _branchRepository.UpdateAsync(branch);
        return new ResultDto
        {
            Success = true,
            Message = "Branch added.",
            Data = _mapper.Map<BranchDto>(branch)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var branch = await _branchRepository.GetByIdAsync(id);
        if (branch == null)
            throw new Exception("Branch not found");

        await _branchRepository.DeleteAsync(branch);
    }
}