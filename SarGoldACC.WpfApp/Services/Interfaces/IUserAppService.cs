using SarGoldACC.Core.DTOs.Auth.User;

namespace SarGoldACC.WpfApp.Services.Interfaces;

public interface IUserAppService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> GetByIdAsync(int id);
    Task AddAsync(UserCreateDto createUserDto);
    Task UpdateAsync(UserUpdateDto updateUserDto);
    Task DeleteAsync(int id);
}