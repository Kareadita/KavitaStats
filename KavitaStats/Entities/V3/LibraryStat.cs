using System;
using System.Collections.Generic;
using KavitaStats.Entities.Enum;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Entities.V3;

public class LibraryStat
{
    public int Id { get; set; }
    public bool IncludeInDashboard { get; set; }
    public bool IncludeInSearch { get; set; }
    public bool UsingFolderWatching { get; set; }
    public bool UsingExcludePatterns { get; set; }
    public bool CreateCollectionsFromMetadata { get; set; }
    public bool CreateReadingListsFromMetadata { get; set; }
    public bool EnabledMetadata { get; set; }
    public LibraryType LibraryType { get; set; }
    public DateTime LastScanned { get; set; }
    public int NumberOfFolders { get; set; }
    public FolderWatchMode? FolderWatchMode { get; set; }
    public FolderWatchMethod? FolderWatchMethod { get; set; }
    /// <summary>
    /// Last non-forced library scan, null when not reported or no library scan since the upgrade
    /// </summary>
    public long? LastScanDurationMs { get; set; }
    /// <summary>
    /// Folders below the roots walked by the last library scan
    /// </summary>
    public int? LastScanFolderCount { get; set; }
    public long? LastScanListMs { get; set; }
    public long? LastScanWalkMs { get; set; }
    public long? LastScanParseMs { get; set; }
    /// <remarks>Counted for https://github.com/Kareadita/Kavita/issues/5007</remarks>
    public int? MultiFileEpubChapterCount { get; set; }

    public List<FileTypeGroup> FileTypes { get; set; }
    
    public int ServerStatId { get; set; }
    public ServerStat ServerStat { get; set; }
}