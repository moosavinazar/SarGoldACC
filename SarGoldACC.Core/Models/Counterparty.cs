namespace SarGoldACC.Core.Models;

public class Counterparty
{
    public long Id { get; set; }
    public GeneralAccount? GeneralAccount { get; set; }
    public long? GeneralAccountId { get; set; }
    public Customer? Customer { get; set; }
    public long? CustomerId { get; set; }
    public ICollection<Invoice> Invoices { get; set; }
    public long BranchId { get; set; }
    public Branch Branch { get; set; }
}