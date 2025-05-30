using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.MadeCategory;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class MadeCategoryService : IMadeCategoryService
{
    
    private readonly IMadeCategoryRepository _madeCategoryRepository;
    private readonly IMapper _mapper;

    public MadeCategoryService(IMadeCategoryRepository madeCategoryRepository, IMapper mapper)
    {
        _madeCategoryRepository = madeCategoryRepository;
        _mapper = mapper;
    }

    public async Task<MadeCategoryDto> GetByIdAsync(long id)
    {
        var madeCategory = await _madeCategoryRepository.GetByIdAsync(id);
        return _mapper.Map<MadeCategoryDto>(madeCategory);
    }

    public async Task<List<MadeCategoryDto>> GetAllAsync()
    {
        var cities = await _madeCategoryRepository.GetAllAsync();
        return _mapper.Map<List<MadeCategoryDto>>(cities);
    }

    public async Task<ResultDto> AddAsync(MadeCategoryCreateDto madeCategoryCreate)
    {
        try
        {
            var madeCategory = new MadeCategory
            {
                Name = madeCategoryCreate.Name,
            };
            await _madeCategoryRepository.AddAsync(madeCategory);
            return new ResultDto
            {
                Success = true,
                Message = "MadeCategory added.",
                Data = _mapper.Map<MadeCategoryDto>(madeCategory)
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

    public async Task<ResultDto> UpdateAsync(MadeCategoryDto madeCategoryDto)
    {
        var madeCategory = await _madeCategoryRepository.GetByIdAsync(madeCategoryDto.Id);
        if (madeCategory == null)
            throw new Exception("MadeCategory not found");

        _mapper.Map(madeCategoryDto, madeCategory);
        await _madeCategoryRepository.UpdateAsync(madeCategory);
        return new ResultDto
        {
            Success = true,
            Message = "MadeCategory added.",
            Data = _mapper.Map<MadeCategoryDto>(madeCategory)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var madeCategory = await _madeCategoryRepository.GetByIdAsync(id);
        if (madeCategory == null)
            throw new Exception("MadeCategory not found");

        await _madeCategoryRepository.DeleteAsync(madeCategory);
    }
}