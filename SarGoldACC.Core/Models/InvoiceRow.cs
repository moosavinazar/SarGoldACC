using SarGoldACC.Core.Enums;

namespace SarGoldACC.Core.Models;

public class InvoiceRow
{
    public long Id { get; set; }
    public long InvoiceId { get; set; }
    public Invoice Invoice { get; set; }
    public string Description { get; set; }
    public string? AdminDescription { get; set; }
    public InvoiceRowAccType AccType { get; set; }
    public OrderAmount? OrderAmount { get; set; }
    public long? OrderAmountId { get; set; }
    public SubMelted? SubMelted { get; set; }
    public long? SubMeltedId { get; set; }
    public Misc? Misc { get; set; }
    public long? MiscId { get; set; }
    public Made? Made { get; set; }
    public long? MadeId { get; set; }
}