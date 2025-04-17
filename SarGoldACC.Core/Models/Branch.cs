using SarGoldACC.Core.Auth.Models;

namespace SarGoldACC.Core.Models;

public class Branch
{
    public long Id { get; set; }
    public required string Name { get; set; }
    
    public ICollection<User> Users { get; set; }
}