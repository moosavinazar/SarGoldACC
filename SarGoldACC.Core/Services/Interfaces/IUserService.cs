using SarGoldACC.Core.DTOs.Auth.User;

namespace SarGoldACC.Core.Services.Interfaces;

public interface IUserService
{
    Task<UserDto> GetByIdAsync(int id);
    Task<List<UserDto>> GetAllAsync();
    Task AddAsync(UserCreateDto createUserDto);
    Task UpdateAsync(UserUpdateDto updateUserDto);
    Task DeleteAsync(int id);
}