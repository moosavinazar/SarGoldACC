using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.Models.Auth;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IMapper _mapper;

    public GroupService(IGroupRepository groupRepository, IMapper mapper)
    {
        _groupRepository = groupRepository;
        _mapper = mapper;
    }

    public async Task<GroupDto> GetByIdAsync(long id)
    {
        var group = await _groupRepository.GetByIdAsync(id);
        return _mapper.Map<GroupDto>(group);
    }

    public async Task<List<GroupDto>> GetAllAsync()
    {
        var groups = await _groupRepository.GetAllAsync();
        return _mapper.Map<List<GroupDto>>(groups);
    }

    public async Task<ResultDto> AddAsync(GroupCreateDto groupCreateDto)
    {
        if (groupCreateDto.GroupPermissions.Count == 0) 
        {
            return new ResultDto
            {
                Success = false,
                Message = "دسترسی انتخاب نشده است",
            };
        }
        try
        {
            var group = new Group
            {
                Name = groupCreateDto.Name,
                Label = groupCreateDto.Label,
                GroupPermissions = groupCreateDto.GroupPermissions
                    .Select(permissionId => new GroupPermission
                    {
                        PermissionId = permissionId
                    }).ToList()
            };
            await _groupRepository.AddAsync(group);
            return new ResultDto
            {
                Success = true,
                Message = "Group added.",
                Data = _mapper.Map<GroupDto>(group)
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

    public async Task UpdateAsync(GroupDto groupDto)
    {
        var user = await _groupRepository.GetByIdAsync(groupDto.Id);
        if (user == null)
            throw new Exception("User not found");

        _mapper.Map(groupDto, user);
        await _groupRepository.UpdateAsync(user);
    }

    public async Task DeleteAsync(long id)
    {
        var user = await _groupRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found");

        await _groupRepository.DeleteAsync(user);
    }
}