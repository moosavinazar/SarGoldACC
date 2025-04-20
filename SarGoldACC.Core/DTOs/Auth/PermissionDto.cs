namespace SarGoldACC.Core.DTOs.Auth;

public class PermissionDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Label { get; set; }
}