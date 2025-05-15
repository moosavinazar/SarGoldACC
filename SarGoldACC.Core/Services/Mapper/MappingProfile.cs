using AutoMapper;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.DTOs.City;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.DTOs.GeneralAccountDto;
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
        CreateMap<City, CityDto>();
        CreateMap<CityDto, City>();
        CreateMap<GeneralAccount, GeneralAccountDto>();
        CreateMap<GeneralAccountDto, GeneralAccount>();
        CreateMap<Counterparty, CounterpartyDto>();
        CreateMap<CounterpartyDto, Counterparty>();
        CreateMap<Customer, CustomerDto>();
        CreateMap<CustomerDto, Customer>();
        CreateMap<Customer, CustomerCreateDto>();
        CreateMap<CustomerCreateDto, Customer>();
        CreateMap<Customer, CustomerUpdateDto>();
        CreateMap<CustomerUpdateDto, Customer>();
        CreateMap<Document, DocumentCreateDto>();
        CreateMap<DocumentCreateDto, Document>();
    }
}