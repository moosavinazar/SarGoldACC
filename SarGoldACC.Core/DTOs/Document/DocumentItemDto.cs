﻿using System.Diagnostics.SymbolStore;
using SarGoldACC.Core.Enums;

namespace SarGoldACC.Core.DTOs.Document;

public class DocumentItemDto
{
    public long CounterpartySideTwoId { get; set; }
    public string CounterpartySideTwoName { get; set; }
    public DocumentItemType Type { get; set; }
    public string TypeTitle { get; set; }
    public string Description { get; set; }
    public double WeightBed { get; set; }
    public double WeightBes { get; set; }
    public double Weight { get; set; }
    public double Weight750 { get; set; }
    public long RiyalBed { get; set; }
    public long RiyalBes { get; set; }
    public long Riyal { get; set; }
    public int? Ayar { get; set; }
    public int? Count { get; set; }
    public bool Certain { get; set; }
    public string? Ang { get; set; }
    public string? Barcode { get; set; }
    public string? Photo { get; set; }
    public long? OjratR { get; set; }
    public double? OjratP { get; set; }
    public string Name { get; set; }
    public long LaboratoryId { get; set; }
    public long BoxId { get; set; }
    public long SubMeltedId { get; set; }
    public long MadeId { get; set; }
    public long MadeSubCategoryId { get; set; }
    public long CoinCategoryId { get; set; }
    
}