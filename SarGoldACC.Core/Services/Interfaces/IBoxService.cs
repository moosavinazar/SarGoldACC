using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.Enums;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IBoxService
{
    Task<BoxDto> GetByIdAsync(long id);
    Task<List<BoxDto>> GetAllAsync();
    Task<List<BoxDto>> GetAllByTypeAsync(BoxType type);
    Task<ResultDto> AddAsync(BoxCreateDto boxCreate);
    Task<ResultDto> UpdateAsync(BoxDto box);
    Task DeleteAsync(long id);
}