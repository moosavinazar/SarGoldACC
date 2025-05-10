namespace SarGoldACC.Core.Models;

public class City
{
    public long Id { get; set; }
    public string Name { get; set; }
    public Customer Customer { get; set; }
}