namespace SarGoldACC.Core.Models;

public class Cash
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public string? Description { get; set; }
    public Counterparty Counterparty { get; set; }
    public Currency Currency { get; set; }
    public long CurrencyId { get; set; }
}