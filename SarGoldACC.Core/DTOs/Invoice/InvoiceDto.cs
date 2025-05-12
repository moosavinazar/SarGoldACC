namespace SarGoldACC.Core.DTOs.Invoice;

public class InvoiceDto
{
    public long Id { get; set; }
    public long DocumentId { get; set; }
    public string Number { get; set; }
    public long CounterpartyId { get; set; }
}