namespace SarGoldACC.Core.Models;

public class InvoiceRow
{
    public long Id { get; set; }
    public long InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
    public string Description { get; set; }
    public string? AdminDescription { get; set; }
}