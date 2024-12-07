using KavitaStats.DTOs.V3;
using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities.V3;

public class LibraryStatFileTypeGroup
{
    public int Id { get; set; }
    public LibraryStat LibraryStat { get; set; }
    public int LibraryStatId { get; set; }

    public FileTypeGroup FileType { get; set; }
}