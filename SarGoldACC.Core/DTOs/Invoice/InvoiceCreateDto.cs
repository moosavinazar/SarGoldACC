namespace SarGoldACC.Core.DTOs.Invoice;

public class InvoiceCreateDto
{
    public long DocumentId { get; set; }
    public string Number { get; set; }
    public long CounterpartyId { get; set; }
}