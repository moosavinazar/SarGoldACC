namespace SarGoldACC.Core.DTOs.Bank;

public class BankCreateDto
{
    public string Name { get; set; }
    public string Branch { get; set; }
    public string Iban { get; set; }
    public string CardNumber { get; set; }
    public string AccountNumber { get; set; }
    public string? Description { get; set; }
    public long BranchId { get; set; }
    public double? WeightBed { get; set; }
    public double? WeightBes { get; set; }
    public long? RiyalBed { get; set; }
    public long? RiyalBes { get; set; }
    public long CurrencyId { get; set; }
}