using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.DTOs.Income;

public class IncomeSeedDataDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public string? Description { get; set; }
}