using KavitaStats.Entities.Enum;

namespace KavitaStats.DTOs;

public class FileFormatDto
{
    /// <summary>
    /// The extension with the ., in lowercase
    /// </summary>
    public string Extension { get; set; }
    /// <summary>
    /// Format of extension
    /// </summary>
    public MangaFormat Format { get; set; }
}