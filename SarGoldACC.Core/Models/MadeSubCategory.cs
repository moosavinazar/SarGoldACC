namespace SarGoldACC.Core.Models;

public class MadeSubCategory
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long MadeCategoryId { get; set; }
    public MadeCategory MadeCategory { get; set; }
    public ICollection<Made> Mades { get; set; }
}