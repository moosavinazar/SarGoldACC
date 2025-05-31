using AutoMapper;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.DTOs.Bank;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.DTOs.Cash;
using SarGoldACC.Core.DTOs.City;
using SarGoldACC.Core.DTOs.CoinCategory;
using SarGoldACC.Core.DTOs.Cost;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Currency;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.DTOs.GeneralAccountDto;
using SarGoldACC.Core.DTOs.Income;
using SarGoldACC.Core.DTOs.Laboratory;
using SarGoldACC.Core.DTOs.MadeCategory;
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
        CreateMap<Counterparty, CounterpartyDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                src.Customer != null ? "مشتری " + src.Customer.Name :
                src.Cash != null ? "صندوق " + src.Cash.Label :
                src.Bank != null ? "بانک " + src.Bank.Name :
                src.Pos != null ? "پوز " + src.Pos.Name :
                src.Laboratory != null ? "ری گیری " + src.Laboratory.Name :
                src.Income != null ? "درآمد " + src.Income.Label :
                src.Cost != null ? "هزینه " + src.Cost.Label :
                src.GeneralAccount != null ? "حساب عمومی " + src.GeneralAccount.Title :
                "null"
            ));
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
        CreateMap<Laboratory, LaboratoryDto>();
        CreateMap<LaboratoryDto, Laboratory>();
        CreateMap<Laboratory, LaboratoryCreateDto>();
        CreateMap<LaboratoryCreateDto, Laboratory>();
        CreateMap<Laboratory, LaboratoryUpdateDto>();
        CreateMap<LaboratoryUpdateDto, Laboratory>();
        CreateMap<Income, IncomeDto>();
        CreateMap<IncomeDto, Income>();
        CreateMap<IncomeCreateDto, Income>();
        CreateMap<Income, IncomeCreateDto>();
        CreateMap<IncomeUpdateDto, Income>();
        CreateMap<Income, IncomeUpdateDto>();
        CreateMap<Cost, CostDto>();
        CreateMap<CostDto, Cost>();
        CreateMap<CostCreateDto, Cost>();
        CreateMap<Cost, CostCreateDto>();
        CreateMap<CostUpdateDto, Cost>();
        CreateMap<Cost, CostUpdateDto>();
        CreateMap<Box, BoxDto>();
        CreateMap<BoxDto, Box>();
        CreateMap<BoxCreateDto, Box>();
        CreateMap<Box, BoxCreateDto>();
        CreateMap<MadeCategory, MadeCategoryDto>();
        CreateMap<MadeCategoryDto, MadeCategory>();
        CreateMap<MadeCategory, MadeCategoryCreateDto>();
        CreateMap<MadeCategoryCreateDto, MadeCategory>();
        CreateMap<MadeSubCategory, MadeSubCategoryDto>();
        CreateMap<MadeSubCategoryDto, MadeSubCategory>();
        CreateMap<MadeSubCategory, MadeSubCategoryCreateDto>();
        CreateMap<MadeSubCategoryCreateDto, MadeSubCategory>();
        CreateMap<CoinCategory, CoinCategoryDto>();
        CreateMap<CoinCategoryDto, CoinCategory>();
        CreateMap<CoinCategory, CoinCategoryCreateDto>();
        CreateMap<CoinCategoryCreateDto, CoinCategory>();
    }
}