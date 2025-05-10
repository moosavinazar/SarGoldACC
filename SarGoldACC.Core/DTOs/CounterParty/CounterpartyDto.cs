using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.DTOs.CounterParty;

public class CounterpartyDto
{
    public long Id { get; set; }
    public GeneralAccount GeneralAccount { get; set; }
    public long GeneralAccountId { get; set; }
}