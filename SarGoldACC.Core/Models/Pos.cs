namespace SarGoldACC.Core.Models;

public class Pos
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string? Description { get; set; }
    public Counterparty Counterparty { get; set; }
    public long BankId { get; set; }
    public Bank Bank { get; set; }
}