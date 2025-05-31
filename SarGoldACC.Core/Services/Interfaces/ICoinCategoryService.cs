using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CoinCategory;

namespace SarGoldACC.Core.Services.Interfaces;

public interface ICoinCategoryService
{
    Task<CoinCategoryDto> GetByIdAsync(long id);
    Task<List<CoinCategoryDto>> GetAllAsync();
    Task<ResultDto> AddAsync(CoinCategoryCreateDto coinCategoryCreate);
    Task<ResultDto> UpdateAsync(CoinCategoryDto coinCategory);
    Task DeleteAsync(long id);
}