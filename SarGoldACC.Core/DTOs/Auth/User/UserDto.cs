using SarGoldACC.Core.Models.Auth;

namespace SarGoldACC.Core.DTOs.Auth.User;

public class UserDto
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public long BranchId { get; set; }
    
    [CsvHelper.Configuration.Attributes.Ignore]
    public string? BranchName { get; set; }
    public ICollection<UserGroup>? UserGroups { get; set; }
}