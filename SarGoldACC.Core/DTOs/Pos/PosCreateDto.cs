namespace SarGoldACC.Core.DTOs.Pos;

public class PosCreateDto
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string? Description { get; set; }
    public long BankId { get; set; }
    public double? WeightBed { get; set; }
    public double? WeightBes { get; set; }
    public long? RiyalBed { get; set; }
    public long? RiyalBes { get; set; }
}