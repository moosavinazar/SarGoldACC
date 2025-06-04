using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.MadeCategory;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class MadeSubCategoryService : IMadeSubCategoryService
{
    
    private readonly IMadeSubCategoryRepository _madeSubCategoryRepository;
    private readonly IMapper _mapper;

    public MadeSubCategoryService(IMadeSubCategoryRepository madeSubCategoryRepository, IMapper mapper)
    {
        _madeSubCategoryRepository = madeSubCategoryRepository;
        _mapper = mapper;
    }

    public async Task<MadeSubCategoryDto> GetByIdAsync(long id)
    {
        var madeSubCategory = await _madeSubCategoryRepository.GetByIdAsync(id);
        return _mapper.Map<MadeSubCategoryDto>(madeSubCategory);
    }

    public async Task<List<MadeSubCategoryDto>> GetAllAsync()
    {
        var cities = await _madeSubCategoryRepository.GetAllAsync();
        return _mapper.Map<List<MadeSubCategoryDto>>(cities);
    }

    public async Task<ResultDto> AddAsync(MadeSubCategoryCreateDto madeSubCategoryCreate)
    {
        try
        {
            var madeSubCategory = new MadeSubCategory()
            {
                Name = madeSubCategoryCreate.Name,
                MadeCategoryId = madeSubCategoryCreate.MadeCategoryId,
            };
            await _madeSubCategoryRepository.AddAsync(madeSubCategory);
            return new ResultDto
            {
                Success = true,
                Message = "MadeSubCategory added.",
                Data = _mapper.Map<MadeSubCategoryDto>(madeSubCategory)
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

    public async Task<ResultDto> UpdateAsync(MadeSubCategoryDto madeSubCategoryDto)
    {
        var madeSubCategory = await _madeSubCategoryRepository.GetByIdAsync(madeSubCategoryDto.Id);
        if (madeSubCategory == null)
            throw new Exception("MadeSubCategory not found");

        _mapper.Map(madeSubCategoryDto, madeSubCategory);
        await _madeSubCategoryRepository.UpdateAsync(madeSubCategory);
        return new ResultDto
        {
            Success = true,
            Message = "MadeSubCategory added.",
            Data = _mapper.Map<MadeSubCategoryDto>(madeSubCategory)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var madeSubCategory = await _madeSubCategoryRepository.GetByIdAsync(id);
        if (madeSubCategory == null)
            throw new Exception("MadeSubCategory not found");

        await _madeSubCategoryRepository.DeleteAsync(madeSubCategory);
    }
}