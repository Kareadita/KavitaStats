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
    public LibraryType LibraryType { get; set; }
    public DateTime LastScanned { get; set; }
    public int NumberOfFolders { get; set; }
    
    public ICollection<LibraryStatFileTypeGroup> FileTypes { get; set; }
    
    public int ServerStatId { get; set; }
    public ServerStat ServerStat { get; set; }
}