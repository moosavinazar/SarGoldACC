using SarGoldACC.Core.Enums;

namespace SarGoldACC.Core.DTOs.Box;

public class BoxCreateDto
{
    public long BranchId { get; set; }
    public string Name { get; set; }
    public double Weight { get; set; }
    public BoxType Type { get; set; }
}