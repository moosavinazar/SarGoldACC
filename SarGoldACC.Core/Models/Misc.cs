namespace SarGoldACC.Core.Models;

public class Misc
{
    public long Id { get; set; }
    public bool Certain { get; set; }
    public int? Ayar { get; set; }
    public double Weight { get; set; }
    public double? Weight750 { get; set; }
    public long BoxId { get; set; }
    public Box Box { get; set; }
    public ICollection<InvoiceRow> InvoiceRows { get; set; }
}