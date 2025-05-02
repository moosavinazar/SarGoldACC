using CsvHelper;
using System.Globalization;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.DTOs.Auth.Group;
using SarGoldACC.Core.DTOs.Auth.User;

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

    private static List<T> ReadCsv<T>(string fileName)
    {
        var filePath = Path.Combine(BasePath, fileName);
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<T>().ToList();
    }
}
