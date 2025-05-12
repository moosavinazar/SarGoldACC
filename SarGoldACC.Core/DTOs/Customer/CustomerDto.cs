namespace SarGoldACC.Core.DTOs.Customer;

public class CustomerDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? IdCode { get; set; }
    public string? Phone { get; set; }
    public string CellPhone { get; set; }
    public string? Address { get; set; }
    public string? Photo { get; set; }
    public string? Moaref { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Email { get; set; }
    public string? StoreName { get; set; }
    public double WeightLimit { get; set; }
    public long RiyalLimit { get; set; }
    public string? Description { get; set; }
    public long CityId { get; set; }
    [CsvHelper.Configuration.Attributes.Ignore]
    public string? CityName { get; set; }
}