using SarGoldACC.Core.Models.Auth;

namespace SarGoldACC.Core.DTOs.Auth.Group;

public class GroupDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Label { get; set; }
    public ICollection<GroupPermission>? GroupPermissions { get; set; }
}