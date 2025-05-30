﻿namespace SarGoldACC.Core.Models.Auth;

public class Permission
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Label { get; set; }
    
    public ICollection<GroupPermission> GroupPermissions { get; set; }
}