﻿namespace SarGoldACC.Core.Models.Auth;

public class User
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public ICollection<UserGroup>? UserGroups { get; set; }
    public long BranchId { get; set; }
    public Branch Branch { get; set; }
}