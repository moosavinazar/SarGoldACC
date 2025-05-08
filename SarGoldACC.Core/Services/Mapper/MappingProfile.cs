using AutoMapper;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Models.Auth;

namespace SarGoldACC.Core.Services.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<User, UserCreateDto>();
        CreateMap<UserCreateDto, User>();
        CreateMap<User, UserUpdateDto>();
        CreateMap<UserUpdateDto, User>();
        CreateMap<Group, GroupDto>();
        CreateMap<GroupDto, Group>();
        CreateMap<Group, GroupCreateDto>();
        CreateMap<GroupCreateDto, Group>();
        CreateMap<Permission, PermissionDto>();
        CreateMap<PermissionDto, Permission>();
        CreateMap<Branch, BranchDto>();
        CreateMap<BranchDto, Branch>();
    }
}