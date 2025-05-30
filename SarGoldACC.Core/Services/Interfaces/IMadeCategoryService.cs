using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.MadeCategory;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IMadeCategoryService
{
    Task<MadeCategoryDto> GetByIdAsync(long id);
    Task<List<MadeCategoryDto>> GetAllAsync();
    Task<ResultDto> AddAsync(MadeCategoryCreateDto cityCreate);
    Task<ResultDto> UpdateAsync(MadeCategoryDto city);
    Task DeleteAsync(long id);
}