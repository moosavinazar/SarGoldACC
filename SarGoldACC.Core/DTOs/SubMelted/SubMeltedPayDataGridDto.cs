using SarGoldACC.Core.Models;

namespace SarGoldACC.Core.DTOs.SubMeltedDto;

public class SubMeltedPayDataGridDto
{
    public long Id { get; set; }
    public string Ang { get; set; }
    public bool Certain { get; set; }
    public int? Ayar { get; set; }
    public double Weight { get; set; }
    public double Weight750 { get; set; }
    public long LaboratoryId { get; set; }
    public string LaboratoryName { get; set; }
    public long BoxId { get; set; }
    public string BoxName { get; set; }
}