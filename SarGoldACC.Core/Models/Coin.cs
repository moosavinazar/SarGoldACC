namespace SarGoldACC.Core.Models;

public class Coin
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int? Ayar { get; set; }
    public double Weight { get; set; }
    public double? Weight750 { get; set; }
    public long? OjratR { get; set; }
    public double? OjratP { get; set; }
    public long CoinCategoryId { get; set; }
    public CoinCategory CoinCategory { get; set; }
    public long BoxId { get; set; }
    public Box Box { get; set; }
    public ICollection<InvoiceRow> InvoiceRows { get; set; }
}