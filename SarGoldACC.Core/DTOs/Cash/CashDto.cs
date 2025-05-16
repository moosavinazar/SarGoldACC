using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.DTOs.Cash;

public class CashDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public string? Description { get; set; }
    public Counterparty Counterparty { get; set; }
    public long CounterpartyId { get; set; }
    public long CurrencyId { get; set; }
}