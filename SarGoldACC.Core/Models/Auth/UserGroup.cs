namespace SarGoldACC.Core.Models.Auth;

public class UserGroup
{
    public long GroupId { get; set; }
    public Group Group { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }
}