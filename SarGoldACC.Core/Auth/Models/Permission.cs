namespace SarGoldACC.Core.Auth.Models;

public class Permission
{
    public long Id { get; set; }
    public required string Name { get; set; }
    
    public ICollection<GroupPermission> GroupPermissions { get; set; }
}