using AutoMapper;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.DTOs.Bank;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.DTOs.Cash;
using SarGoldACC.Core.DTOs.City;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Currency;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.DTOs.GeneralAccountDto;
using SarGoldACC.Core.DTOs.Pos;
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
        CreateMap<Bank, BankDto>();
        CreateMap<BankDto, Bank>();
        CreateMap<BankCreateDto, Bank>();
        CreateMap<Bank, BankCreateDto>();
        CreateMap<BankUpdateDto, Bank>();
        CreateMap<Bank, BankUpdateDto>();
        CreateMap<Pos, PosDto>();
        CreateMap<PosDto, Pos>();
        CreateMap<PosCreateDto, Pos>();
        CreateMap<Pos, PosCreateDto>();
        CreateMap<PosUpdateDto, Pos>();
        CreateMap<Pos, PosUpdateDto>();
        CreateMap<Currency, CurrencyDto>();
        CreateMap<CurrencyDto, Currency>();
        CreateMap<CurrencyCreateDto, Currency>();
        CreateMap<Currency, CurrencyCreateDto>();
        CreateMap<Cash, CashDto>();
        CreateMap<CashDto, Cash>();
        CreateMap<CashCreateDto, Cash>();
        CreateMap<Cash, CashCreateDto>();
        CreateMap<CashUpdateDto, Cash>();
        CreateMap<Cash, CashUpdateDto>();
    }
}