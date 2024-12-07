namespace KavitaStats.Entities.V3;

public class UserStatRole
{
    public int Id { get; set; }
    public required string Role { get; set; }
    
    public int UserStatId { get; set; }
    public virtual UserStat UserStat { get; set; }
}