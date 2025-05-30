using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.MadeCategory;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IMadeSubCategoryService
{
    Task<MadeSubCategoryDto> GetByIdAsync(long id);
    Task<List<MadeSubCategoryDto>> GetAllAsync();
    Task<ResultDto> AddAsync(MadeSubCategoryCreateDto cityCreate);
    Task<ResultDto> UpdateAsync(MadeSubCategoryDto city);
    Task DeleteAsync(long id);
}