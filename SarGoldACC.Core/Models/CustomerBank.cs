namespace SarGoldACC.Core.Models;

public class CustomerBank
{
    public long Id { get; set; }
    public string Iban { get; set; }
    public string CardNumber { get; set; }
    public string AccountNumber { get; set; }
    public string Name { get; set; }
    public Customer Customer { get; set; }
    public long CustomerId { get; set; }
}