using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.DTOs.CounterParty;

public class CounterpartyDto
{
    public long Id { get; set; }
    public long BranchId { get; set; }
    public long? GeneralAccountId { get; set; }
    public long? CustomerId{ get; set; }
}