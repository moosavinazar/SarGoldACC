namespace SarGoldACC.Core.Models;

public class Made
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int? Ayar { get; set; }
    public double Weight { get; set; }
    public double? Weight750 { get; set; }
    public string? Barcode { get; set; }
    public string? Photo { get; set; }
    public long? OjratR { get; set; }
    public double? OjratP { get; set; }
    public string? Description { get; set; }
    public long MadeSubCategoryId { get; set; }
    public MadeSubCategory MadeSubCategory { get; set; }
    public long BoxId { get; set; }
    public Box Box { get; set; }
    public ICollection<InvoiceRow> InvoiceRows { get; set; }
}