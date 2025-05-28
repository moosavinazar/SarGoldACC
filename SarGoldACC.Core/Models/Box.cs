using SarGoldACC.Core.Enums;

namespace SarGoldACC.Core.Models;

public class Box
{
    public long Id { get; set; }
    public long BranchId { get; set; }
    public Branch Branch { get; set; }
    public string Name { get; set; }
    public double Weight { get; set; }
    public BoxType Type { get; set; }
    public ICollection<SubMelted> SubMelteds { get; set; }
    public ICollection<Misc> Miscs { get; set; }
}