using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Laboratory;

namespace SarGoldACC.Core.Services.Interfaces;

public interface ILaboratoryService
{
    Task<LaboratoryDto> GetByIdAsync(long id);
    Task<List<LaboratoryDto>> GetAllAsync();
    Task<ResultDto> AddAsync(LaboratoryCreateDto laboratoryCreate);
    Task<ResultDto> UpdateAsync(LaboratoryUpdateDto laboratoryUpdate);
    Task DeleteAsync(long id);
}