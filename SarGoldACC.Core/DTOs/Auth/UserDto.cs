namespace SarGoldACC.Core.DTOs.Auth;

public class UserDto
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public long BranchId { get; set; }
}