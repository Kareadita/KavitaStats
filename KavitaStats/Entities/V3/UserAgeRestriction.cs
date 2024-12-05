using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities.V3;

public class UserAgeRestriction
{
    public int Id { get; set; }
    public AgeRating AgeRating { get; set; }
    public bool IncludeUnknowns { get; set; }
    
    public int UserStatId { get; set; }
    public UserStat User { get; set; }
}