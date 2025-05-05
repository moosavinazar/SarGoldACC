namespace SarGoldACC.Core.DTOs.Auth.Group;

public class GroupCreateDto
{
    public required string Name { get; set; }
    public required string Label { get; set; }
    public required ICollection<long> GroupPermissions { get; set; }
}