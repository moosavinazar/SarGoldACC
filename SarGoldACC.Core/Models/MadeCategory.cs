namespace SarGoldACC.Core.Models;

public class MadeCategory
{
    public long Id { get; set; }
    public string Name { get; set; }
    public ICollection<MadeSubCategory> MadeSubCategories { get; private set; }
}