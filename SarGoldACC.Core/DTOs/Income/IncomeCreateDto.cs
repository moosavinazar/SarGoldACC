namespace SarGoldACC.Core.DTOs.Income;

public class IncomeCreateDto
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
}