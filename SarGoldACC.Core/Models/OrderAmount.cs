namespace SarGoldACC.Core.Models;

public class OrderAmount
{
    public long Id { get; set; }
    public string Description {get; set;}
    public double Weight { get; set; }
    public long Riyal { get; set; }
    public ICollection<InvoiceRow> InvoiceRows { get; set; }
}