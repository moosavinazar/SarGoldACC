namespace SarGoldACC.Core.Auth.Models;

public class Group
{
    public long Id { get; set; }
    public required string Name { get; set; }
    
    public ICollection<GroupPermission>? GroupPermissions { get; set; }
    public ICollection<User>? Users { get; set; }
}