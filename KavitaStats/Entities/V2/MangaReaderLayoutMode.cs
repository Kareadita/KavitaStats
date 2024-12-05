using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities;

public class MangaReaderLayoutMode
{
    public int Id { get; set; }
    public ReaderMode ReaderMode { get; set; }
    
    public virtual int StatRecordId { get; set; }
    public virtual StatRecord StatRecord { get; set; }
}