using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Box;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IBoxService
{
    Task<BoxDto> GetByIdAsync(long id);
    Task<List<BoxDto>> GetAllAsync();
    Task<ResultDto> AddAsync(BoxCreateDto boxCreate);
    Task<ResultDto> UpdateAsync(BoxDto box);
    Task DeleteAsync(long id);
}