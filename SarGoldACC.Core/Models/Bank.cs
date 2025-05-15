namespace SarGoldACC.Core.Models;

public class Bank
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Branch { get; set; }
    public string Iban { get; set; }
    public string CardNumber { get; set; }
    public string AccountNumber { get; set; }
    public string Description { get; set; }
    public Counterparty Counterparty { get; set; }
    public Currency Currency { get; set; }
    public long CurrencyId { get; set; }
    public ICollection<Pos> Pos { get; set; }
}