namespace SarGoldACC.Core.Models;

public class Melted
{
    public long Id { get; set; }
    public string Ang { get; set; }
    public bool Certain { get; set; }
    public int? Ayar { get; set; }
    public long LaboratoryId { get; set; }
    public Laboratory Laboratory { get; set; }
    public ICollection<SubMelted> SubMelteds { get; set; }
}