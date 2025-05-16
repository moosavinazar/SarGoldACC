namespace SarGoldACC.Core.DTOs.Cash;

public class CashCreateDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Label { get; set; }
    public string? Description { get; set; }
    public long BranchId { get; set; }
    public double? WeightBed { get; set; }
    public double? WeightBes { get; set; }
    public long? RiyalBed { get; set; }
    public long? RiyalBes { get; set; }
    public long CurrencyId { get; set; }
}