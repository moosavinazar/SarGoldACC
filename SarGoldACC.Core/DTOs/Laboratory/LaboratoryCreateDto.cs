namespace SarGoldACC.Core.DTOs.Laboratory;

public class LaboratoryCreateDto
{
    public string Name { get; set; }
    public string Photo { get; set; }
    public string Phone { get; set; }
    public string CellPhone { get; set; }
    public string IVRPhone { get; set; }
    public string Description { get; set; }
    public long BranchId { get; set; }
    public double? WeightBed { get; set; }
    public double? WeightBes { get; set; }
    public long? RiyalBed { get; set; }
    public long? RiyalBes { get; set; }
    public long CityId { get; set; }
}