namespace SarGoldACC.Core.Models;

public class GeneralAccount
{
    public long Id { get; set; }
    public string Title { get; set; }
    public ICollection<Counterparty> Counterparties { get; set; }
}