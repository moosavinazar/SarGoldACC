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
    public long RiyalBed { get; set; }
    public long RiyalBes { get; set; }
    
}