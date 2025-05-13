namespace SarGoldACC.Core.DTOs.Document;

public class CounterPartyOpeningEntryDto
{
    public long counterpartyId { get; set; }
    public long branchId { get; set; }
    public double WeightBed { get; set; }
    public double WeightBes { get; set; }
    public long RiyalBed { get; set; }
    public long RiyalBes { get; set; }
}