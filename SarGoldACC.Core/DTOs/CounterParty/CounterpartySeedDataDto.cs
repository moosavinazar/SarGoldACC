using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.DTOs.CounterParty;

public class CounterpartySeedDataDto
{
    public long Id { get; set; }
    public long? GeneralAccountId { get; set; }
    public long? CustomerId { get; set; }
    public long? CashId { get; set; }
    public long BranchId { get; set; }
}