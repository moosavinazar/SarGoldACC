namespace SarGoldACC.Core.Models;

public class Invoice
{
    public long DocumentId { get; set; }
    public Document Document { get; set; }
    public string Number { get; set; }
    public ICollection<InvoiceRow> InvoiceRows { get; set; }
}