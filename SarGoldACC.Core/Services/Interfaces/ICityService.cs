using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.DTOs.City;

namespace SarGoldACC.Core.Services.Interfaces;

public interface ICityService
{
    Task<CityDto> GetByIdAsync(long id);
    Task<List<CityDto>> GetAllAsync();
    Task<ResultDto> AddAsync(CityCreateDto branchCreate);
    Task<ResultDto> UpdateAsync(CityDto branch);
    Task DeleteAsync(long id);
}