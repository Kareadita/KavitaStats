using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities;

public class AgeRestriction
{
    public AgeRating AgeRating { get; set; }
    public bool IncludeUnknowns { get; set; }
}
