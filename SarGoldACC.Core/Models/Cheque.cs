using SarGoldACC.Core.Enums;

namespace SarGoldACC.Core.Models;

public class Cheque
{
    public long Id { get; set; }
    public string Serial { get; set; }
    public long Amount { get; set; }
    public string BankName { get; set; }
    public string BranchName { get; set; }
    public DateTime DoneDate { get; set; }
    public DateTime ExportDate { get; set; }
    public bool Guarantee { get; set; }
    public ChequeSatatus Satatus { get; set; }
    public ChequeType Type { get; set; }
    public Counterparty Drawer { get; set; }
    public long DrawerId { get; set; }
    public Counterparty Payee { get; set; }
    public long PayeeId { get; set; }
}