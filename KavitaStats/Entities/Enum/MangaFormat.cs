using System.ComponentModel;

namespace KavitaStats.Entities.Enum;

/// <summary>
/// Represents the format of the file
/// </summary>
public enum MangaFormat
{
    /// <summary>
    /// Image file
    /// </summary>
    [Description("Image")]
    Image = 0,
    /// <summary>
    /// Archive based file
    /// </summary>
    [Description("Archive")]
    Archive = 1,
    /// <summary>
    /// Unknown. Not used.
    /// </summary>
    [Description("Unknown")]
    Unknown = 2,
    /// <summary>
    /// EPUB File
    /// </summary>
    [Description("EPUB")]
    Epub = 3,
    /// <summary>
    /// PDF File
    /// </summary>
    [Description("PDF")]
    Pdf = 4
}