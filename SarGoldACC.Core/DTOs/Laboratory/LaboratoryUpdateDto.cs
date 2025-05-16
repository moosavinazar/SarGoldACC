namespace SarGoldACC.Core.DTOs.Laboratory;

public class LaboratoryUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Photo { get; set; }
    public string Phone { get; set; }
    public string CellPhone { get; set; }
    public string IVRPhone { get; set; }
    public string Description { get; set; }
    public long CityId { get; set; }
}