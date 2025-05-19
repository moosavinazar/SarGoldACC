namespace SarGoldACC.Core.Models;

public class OrderAmount
{
    public long Id { get; set; }
    public string Description {get; set;}
    public double WeightBed { get; set; }
    public double WeightBes { get; set; }
    public long RiyalBed { get; set; }
    public long RiyalBes { get; set; }
    public InvoiceRow InvoiceRow { get; set; }
}