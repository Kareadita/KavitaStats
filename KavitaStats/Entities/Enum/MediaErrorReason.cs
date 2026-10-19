using System.ComponentModel;

namespace KavitaStats.Entities.Enum;

/// <summary>
/// Copy of Kavita's MediaErrorReason, same values. 4 is unused there too
/// </summary>
public enum MediaErrorReason
{
    [Description("Unknown")]
    Unknown = 0,
    [Description("Parse Failed")]
    ParseFailed = 1,
    [Description("No Series Name")]
    NoSeriesName = 2,
    [Description("Unreadable Archive")]
    UnreadableArchive = 3,
    [Description("Corrupt Epub")]
    CorruptEpub = 5,
    [Description("No Pages")]
    NoPages = 6,
    [Description("Cover Failed")]
    CoverFailed = 7,
    [Description("Word Count Failed")]
    WordCountFailed = 8,
    [Description("I/O Error")]
    IoError = 9,
    [Description("Metadata Unreadable")]
    MetadataUnreadable = 10,
    [Description("Epub Not Strict")]
    EpubNotStrict = 11,
    [Description("Ambiguous Series")]
    AmbiguousSeries = 12,
}
