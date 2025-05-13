namespace SarGoldACC.Core.Models;

public class GeneralAccountAmount
{
    public long Id { get; set; }
    public double WeightBed { get; set; }
    public double WeightBes { get; set; }
    public long RiyalBed { get; set; }
    public long RiyalBes { get; set; }
    public InvoiceRow InvoiceRow { get; set; }
}