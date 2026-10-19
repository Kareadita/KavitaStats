using KavitaStats.Entities.Enum;

namespace KavitaStats.DTOs.V3;

public class MediaErrorStatV3
{
    public MediaErrorReason Reason { get; set; }
    public int Count { get; set; }
    public int DismissedCount { get; set; }
}
