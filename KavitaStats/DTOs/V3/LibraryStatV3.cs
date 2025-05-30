﻿using System;
using System.Collections.Generic;
using KavitaStats.DTOs.V2;
using KavitaStats.Entities.Enum;
using KavitaStats.Entities.V3;

namespace KavitaStats.DTOs.V3;

public class LibraryStatV3
{
    public bool IncludeInDashboard { get; set; }
    public bool IncludeInSearch { get; set; }
    public bool UsingFolderWatching { get; set; }
    /// <summary>
    /// Are any exclude patterns setup
    /// </summary>
    public bool UsingExcludePatterns { get; set; }
    /// <summary>
    /// Will this library create collections from ComicInfo
    /// </summary>
    public bool CreateCollectionsFromMetadata { get; set; }
    /// <summary>
    /// Will this library create reading lists from ComicInfo
    /// </summary>
    public bool CreateReadingListsFromMetadata { get; set; }
    /// <summary>
    /// Type of the Library
    /// </summary>
    public LibraryType LibraryType { get; set; }
    public List<FileTypeGroup> FileTypes { get; set; }
    /// <summary>
    /// Last time library was fully scanned
    /// </summary>
    public DateTime LastScanned { get; set; }
    /// <summary>
    /// Number of folders the library has
    /// </summary>
    public int NumberOfFolders { get; set; }
}
