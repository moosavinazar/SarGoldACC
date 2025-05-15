using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Pos;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IPosService
{
    Task<PosDto> GetByIdAsync(long id);
    Task<List<PosDto>> GetAllAsync();
    Task<ResultDto> AddAsync(PosCreateDto posCreate);
    Task<ResultDto> UpdateAsync(PosDto pos);
    Task DeleteAsync(long id);
}