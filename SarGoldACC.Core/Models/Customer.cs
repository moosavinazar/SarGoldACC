namespace SarGoldACC.Core.Models;

public class Customer
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string IdCode { get; set; }
    public string Phone { get; set; }
    public string CellPhone { get; set; }
    public string Address { get; set; }
    public string Photo { get; set; } // Assuming 'image' translates to byte[] in C#
    public string Moaref { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Email { get; set; }
    public string StoreName { get; set; }
    public double WeightLimit { get; set; }
    public long RiyalLimit { get; set; }
    public string Description { get; set; }
    
    public City City { get; set; }
    public long CityId { get; set; }
    public ICollection<CustomerBank> CustomerBanks { get; set; }
    public Counterparty Counterparty { get; set; }
}