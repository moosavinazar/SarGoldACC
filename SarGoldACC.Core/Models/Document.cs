using SarGoldACC.Core.Enums;

namespace SarGoldACC.Core.Models;

public class Document
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public DocumentType Type { get; set; }
    public ICollection<Invoice> Invoices { get; set; }
}