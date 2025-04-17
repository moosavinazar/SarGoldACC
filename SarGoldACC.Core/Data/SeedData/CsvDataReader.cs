using CsvHelper;
using System.Globalization;
using SarGoldACC.Core.Auth.DTOs;
using SarGoldACC.Core.DTOs;

namespace SarGoldACC.Core.Data.SeedData;

public class CsvDataReader
{
    private static readonly string BasePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())?.FullName ?? string.Empty, "SarGoldACC.Core/Data/SeedData");

    public static List<BranchDto> ReadBranches()
    {
        using var reader = new StreamReader(Path.Combine(BasePath, "Branches.csv"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<BranchDto>().ToList();
    }
    
    public static List<GroupDto> ReadGroups()
    {
        using var reader = new StreamReader(Path.Combine(BasePath, "Groups.csv"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<GroupDto>().ToList();
    }
    
    public static List<PermissionDto> ReadPermissions()
    {
        using var reader = new StreamReader(Path.Combine(BasePath, "Permissions.csv"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<PermissionDto>().ToList();
    }
    
    public static List<GroupPermissionDto> ReadGroupPermissions()
    {
        using var reader = new StreamReader(Path.Combine(BasePath, "GroupPermissions.csv"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<GroupPermissionDto>().ToList();
    }
    
    public static List<UserDto> ReadUsers()
    {
        using var reader = new StreamReader(Path.Combine(BasePath, "Users.csv"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<UserDto>().ToList();
    }
}