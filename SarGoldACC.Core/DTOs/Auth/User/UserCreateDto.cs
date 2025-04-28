namespace SarGoldACC.Core.DTOs.Auth.User;

public class UserCreateDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public long BranchId { get; set; }
}