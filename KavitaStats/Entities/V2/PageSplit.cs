using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities;

public class PageSplit
{
    public int Id { get; set; }
    public PageSplitOption PageSplitOption { get; set; }
    
    public virtual int StatRecordId { get; set; }
    public virtual StatRecord StatRecord { get; set; }
}
