using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Auth.User;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(long id);
    Task<List<UserDto>> GetAllAsync();
    Task<ResultDto> AddAsync(UserCreateDto createUserDto);
    Task<ResultDto> UpdateAsync(UserUpdateDto updateUserDto);
    Task DeleteAsync(long id);
}