using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> GetByIdAsync(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<ResultDto> AddAsync(UserCreateDto createUserDto)
    {
        if (createUserDto.UserGroups.Count == 0) 
        {
            return new ResultDto
            {
                Success = false,
                Message = "گروه کاربری انتخاب نشده است",
            };
        }
        try
        {
            var user = new User
            {
                Username = createUserDto.Username,
                Password = createUserDto.Password,
                Name = createUserDto.Name,
                PhoneNumber = createUserDto.PhoneNumber,
                BranchId = createUserDto.BranchId,
                UserGroups = createUserDto.UserGroups
                    .Select(groupId => new UserGroup
                    {
                        GroupId = groupId
                    }).ToList()
            };
            await _userRepository.AddAsync(user);
            return new ResultDto
            {
                Success = true,
                Message = "User added.",
                Data = _mapper.Map<UserDto>(user)
            };
        }
        catch (Exception ex)
        {
            return new ResultDto
            {
                Success = false,
                Message = ex.Message,
            };
        }
    }

    public async Task<ResultDto> UpdateAsync(UserUpdateDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(updateUserDto.Id);
        if (user == null)
            throw new Exception("User not found");

        _mapper.Map(updateUserDto, user);
        await _userRepository.UpdateAsync(user);
        return new ResultDto
        {
            Success = true,
            Message = "user updated.",
            Data = _mapper.Map<UserDto>(user)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found");

        await _userRepository.DeleteAsync(user);
    }
}