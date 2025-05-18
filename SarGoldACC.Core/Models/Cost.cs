namespace SarGoldACC.Core.Models;

public class Cost
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public string? Description { get; set; }
    public Counterparty Counterparty { get; set; }
}