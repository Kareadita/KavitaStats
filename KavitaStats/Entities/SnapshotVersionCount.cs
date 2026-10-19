namespace KavitaStats.Entities;

public class SnapshotVersionCount
{
    public int Id { get; set; }
    public int HistoricalSnapshotId { get; set; }
    public string KavitaVersion { get; set; }
    public int Count { get; set; }
}
