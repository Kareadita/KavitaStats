using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities;

public class FileFormat
{
    public int Id { get; set; }
    /// <summary>
    /// The extension with the ., in lowercase
    /// </summary>
    public string Extension { get; set; }
    /// <summary>
    /// Format of extension
    /// </summary>
    public MangaFormat Format { get; set; }
}