using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.DTOs.CounterParty;

public class CounterpartyDto
{
    public long Id { get; set; }
    public long BranchId { get; set; }
    public long? GeneralAccountId { get; set; }
    public GeneralAccount? GeneralAccount { get; set; }
    public long? CustomerId{ get; set; }
    public Models.Customer? Customer { get; set; }
    public long? BankId{ get; set; }
    public Models.Bank? Bank { get; set; }
    public long? PosId{ get; set; }
    public Models.Pos? Pos { get; set; }
    
    public long? CashId{ get; set; }
    public Models.Cash? Cash { get; set; }
}