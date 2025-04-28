using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Services.Interfaces;

namespace SarGoldACC.WpfApp.Services;

public class UserAppService : IUserAppService
{
    private readonly IUserService _userService;

    public UserAppService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _userService.GetAllAsync();
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        return await _userService.GetByIdAsync(id);
    }

    public async Task AddAsync(UserCreateDto createUserDto)
    {
        await _userService.AddAsync(createUserDto);
    }

    public async Task UpdateAsync(UserUpdateDto updateUserDto)
    {
        await _userService.UpdateAsync(updateUserDto);
    }

    public async Task DeleteAsync(int id)
    {
        await _userService.DeleteAsync(id);
    }
}