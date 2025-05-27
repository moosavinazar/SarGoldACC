namespace SarGoldACC.Core.Models;

public class SubMelted
{
    public long Id { get; set; }
    public long MeltedId { get; set; }
    public Melted Melted { get; set; }
    public long BoxId { get; set; }
    public Box Box { get; set; }
}