namespace SarGoldACC.Core.Models;

public class Counterparty
{
    public long Id { get; set; }
    public GeneralAccount? GeneralAccount { get; set; }
    public long? GeneralAccountId { get; set; }
    public Customer? Customer { get; set; }
    public long? CustomerId { get; set; }
    public Cash? Cash { get; set; }
    public long? CashId { get; set; }
    public Bank? Bank { get; set; }
    public long? BankId { get; set; }
    public Pos? Pos { get; set; }
    public long? PosId { get; set; }
    public Laboratory? Laboratory { get; set; }
    public long? LaboratoryId { get; set; }
    public ICollection<Invoice> Invoices { get; set; }
    public long BranchId { get; set; }
    public Branch Branch { get; set; }
    // صادر کننده چک
    public ICollection<Cheque> Drawers { get; set; }
    // دریافت کننده چک
    public ICollection<Cheque> Payees { get; set; }
}