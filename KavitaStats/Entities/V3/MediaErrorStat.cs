using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities.V3;

public class MediaErrorStat
{
    public int Id { get; set; }
    public MediaErrorReason Reason { get; set; }
    public int Count { get; set; }
    public int DismissedCount { get; set; }

    public int ServerStatId { get; set; }
    public virtual ServerStat ServerStat { get; set; }
}
