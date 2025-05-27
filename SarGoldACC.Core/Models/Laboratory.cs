namespace SarGoldACC.Core.Models;

public class Laboratory
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Photo { get; set; }
    public string? Phone { get; set; }
    public string? CellPhone { get; set; }
    public string? IVRPhone { get; set; }
    public string? Description { get; set; }
    public City City { get; set; }
    public long CityId { get; set; }
    public Counterparty Counterparty { get; set; }
    public ICollection<Melted> Melteds { get; set; }
}