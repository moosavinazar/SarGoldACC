using CsvHelper;
using System.Globalization;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.DTOs.City;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Currency;
using SarGoldACC.Core.DTOs.GeneralAccountDto;
using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.Data.SeedData;

public class CsvDataReader
{
    private static readonly string BasePath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData");

    public static List<BranchDto> ReadBranches() =>
        ReadCsv<BranchDto>("Branches.csv");

    public static List<GroupDto> ReadGroups() =>
        ReadCsv<GroupDto>("Groups.csv");

    public static List<PermissionDto> ReadPermissions() =>
        ReadCsv<PermissionDto>("Permissions.csv");

    public static List<GroupPermissionDto> ReadGroupPermissions() =>
        ReadCsv<GroupPermissionDto>("GroupPermissions.csv");

    public static List<UserDto> ReadUsers() =>
        ReadCsv<UserDto>("Users.csv");

    public static List<UserGroupDto> ReadUserGroup() =>
        ReadCsv<UserGroupDto>("UserGroup.csv");
    
    public static List<CityDto> ReadCity() =>
        ReadCsv<CityDto>("City.csv");
    
    public static List<GeneralAccountDto> ReadGeneralAccount() =>
        ReadCsv<GeneralAccountDto>("GeneralAccount.csv");
    
    public static List<CounterpartySeedDataDto> ReadCounterparty() =>
        ReadCsv<CounterpartySeedDataDto>("Counterparty.csv");
    
    public static List<CurrencyDto> ReadCurrency() =>
        ReadCsv<CurrencyDto>("Currency.csv");

    private static List<T> ReadCsv<T>(string fileName)
    {
        var filePath = Path.Combine(BasePath, fileName);
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<T>().ToList();
    }
}
