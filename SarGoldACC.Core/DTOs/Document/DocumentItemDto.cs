using System.Diagnostics.SymbolStore;
using SarGoldACC.Core.Enums;

namespace SarGoldACC.Core.DTOs.Document;

public class DocumentItemDto
{
    public long CounterpartySideTwoId { get; set; }
    public DocumentItemType Type { get; set; }
    public string Description { get; set; }
    public double WeightBed { get; set; }
    public double WeightBes { get; set; }
    public double? Weight750 { get; set; }
    public long RiyalBed { get; set; }
    public long RiyalBes { get; set; }
    public int? Ayar { get; set; }
    public bool Certain { get; set; }
    public string Ang { get; set; }
    public long LaboratoryId { get; set; }
    public long BoxId { get; set; }
    public long SubMeltedId { get; set; }
    
}