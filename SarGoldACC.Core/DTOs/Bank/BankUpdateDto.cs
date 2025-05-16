namespace SarGoldACC.Core.DTOs.Bank;

public class BankUpdateDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Branch { get; set; }
    public string Iban { get; set; }
    public string CardNumber { get; set; }
    public string AccountNumber { get; set; }
    public string Description { get; set; }
}