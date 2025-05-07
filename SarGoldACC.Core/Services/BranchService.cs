using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class BranchService : IBranchService
{
    
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public BranchService(IBranchRepository branchRepository, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
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
        try
        {
            var branch = new Branch
            {
                Name = branchCreate.Name,
            };
            await _branchRepository.AddAsync(branch);
            return new ResultDto
            {
                Success = true,
                Message = "Branch added.",
                Data = _mapper.Map<BranchDto>(branch)
            };
        }
        catch (Exception ex)
        {
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