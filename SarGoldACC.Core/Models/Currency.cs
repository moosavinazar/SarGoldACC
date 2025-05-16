namespace SarGoldACC.Core.Models;

public class Currency
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public ICollection<Bank> Banks { get; set; }
    public ICollection<Cash> Cash { get; set; }
}