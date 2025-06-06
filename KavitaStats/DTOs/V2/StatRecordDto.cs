﻿using System;
using System.Collections.Generic;
using KavitaStats.DTOs.V2;
using KavitaStats.Entities.Enum;

namespace KavitaStats.DTOs;

/// <summary>
/// Represents information about a Kavita Installation
/// </summary>
public class StatRecordDto
{
    /// <summary>
    /// Unique Id that represents a unique install
    /// </summary>
    public string InstallId { get; set; }
    public string Os { get; set; }
    /// <summary>
    /// If the Kavita install is using Docker
    /// </summary>
    public bool IsDocker { get; set; }
    /// <summary>
    /// Version of .NET instance is running
    /// </summary>
    public string DotnetVersion { get; set; }
    /// <summary>
    /// Version of Kavita
    /// </summary>
    public string KavitaVersion { get; set; }
    /// <summary>
    /// Number of Cores on the instance
    /// </summary>
    public int NumOfCores { get; set; }
    /// <summary>
    /// The number of libraries on the instance
    /// </summary>
    public int NumberOfLibraries { get; set; }
    /// <summary>
    /// Does any user have bookmarks
    /// </summary>
    public bool HasBookmarks { get; set; }
    /// <summary>
    /// The site theme the install is using
    /// </summary>
    /// <remarks>Introduced in v0.5.2</remarks>
    public string ActiveSiteTheme { get; set; }
    /// <summary>
    /// The reading mode the main user has as a preference
    /// </summary>
    /// <remarks>Introduced in v0.5.2</remarks>
    public ReaderMode MangaReaderMode { get; set; }
    /// <summary>
    /// Number of users on the install
    /// </summary>
    /// <remarks>Introduced in v0.5.2</remarks>
    public int NumberOfUsers { get; set; }
    /// <summary>
    /// Number of collections on the install
    /// </summary>
    /// <remarks>Introduced in v0.5.2</remarks>
    public int NumberOfCollections { get; set; }
    /// <summary>
    /// Number of reading lists on the install (Sum of all users)
    /// </summary>
    /// <remarks>Introduced in v0.5.2</remarks>
    public int NumberOfReadingLists { get; set; }
    /// <summary>
    /// Is OPDS enabled
    /// </summary>
    /// <remarks>Introduced in v0.5.2</remarks>
    public bool OPDSEnabled { get; set; }
    /// <summary>
    /// Total number of files in the instance
    /// </summary>
    /// <remarks>Introduced in v0.5.2</remarks>
    public int TotalFiles { get; set; }
    /// <summary>
    /// Total number of Genres in the instance
    /// </summary>
    /// <remarks>Introduced in v0.5.4</remarks>
    public int TotalGenres { get; set; }
    /// <summary>
    /// Total number of People in the instance
    /// </summary>
    /// <remarks>Introduced in v0.5.4</remarks>
    public int TotalPeople { get; set; }
    /// <summary>
    /// Number of users on this instance using Card Layout
    /// </summary>
    /// <remarks>Introduced in v0.5.4</remarks>
    public int UsersOnCardLayout { get; set; }
    /// <summary>
    /// Number of users on this instance using List Layout
    /// </summary>
    /// <remarks>Introduced in v0.5.4</remarks>
    public int UsersOnListLayout { get; set; }
    /// <summary>
    /// Max number of Series for any library on the instance
    /// </summary>
    /// <remarks>Introduced in v0.5.4</remarks>
    public int MaxSeriesInALibrary { get; set; }
    /// <summary>
    /// Max number of Volumes for any library on the instance
    /// </summary>
    /// <remarks>Introduced in v0.5.4</remarks>
    public int MaxVolumesInASeries { get; set; }
    /// <summary>
    /// Max number of Chapters for any library on the instance
    /// </summary>
    /// <remarks>Introduced in v0.5.4</remarks>
    public int MaxChaptersInASeries { get; set; }
    /// <summary>
    /// Does this instance have relationships setup between series
    /// </summary>
    /// <remarks>Introduced in v0.5.4</remarks>
    public bool UsingSeriesRelationships { get; set; }

    /// <summary>
    /// A list of background colors set on the instance
    /// </summary>
    /// <remarks>Introduced in v0.6.0</remarks>
    public ICollection<string> MangaReaderBackgroundColors { get; set; } = Array.Empty<string>();
    /// <summary>
    /// A list of Page Split defaults being used on the instance
    /// </summary>
    /// <remarks>Introduced in v0.6.0</remarks>
    public ICollection<PageSplitOption> MangaReaderPageSplittingModes { get; set; } = Array.Empty<PageSplitOption>();
    /// <summary>
    /// A list of Layout Mode defaults being used on the instance
    /// </summary>
    /// <remarks>Introduced in v0.6.0</remarks>
    public ICollection<ReaderMode> MangaReaderLayoutModes { get; set; } = Array.Empty<ReaderMode>();
    /// <summary>
    /// A list of file formats existing in the instance
    /// </summary>
    /// <remarks>Introduced in v0.6.0</remarks>
    public ICollection<FileFormatDto> FileFormats { get; set; } = Array.Empty<FileFormatDto>();
    /// <summary>
    /// If there is at least one user that is using an age restricted profile on the instance
    /// </summary>
    /// <remarks>Introduced in v0.6.0</remarks>
    public bool UsingRestrictedProfiles { get; set; }
    /// <summary>
    /// Number of users using the Emulate Comic Book setting
    /// </summary>
    /// <remarks>Introduced in v0.7.0</remarks>
    public int UsersWithEmulateComicBook { get; set; }
    /// <summary>
    /// Percent (0.0-1.0) of libraries with folder watching enabled
    /// </summary>
    /// <remarks>Introduced in v0.7.0</remarks>
    public float PercentOfLibrariesWithFolderWatchingEnabled { get; set; }
    /// <summary>
    /// Percent (0.0-1.0) of libraries included in Search
    /// </summary>
    /// <remarks>Introduced in v0.7.0</remarks>
    public float PercentOfLibrariesIncludedInSearch { get; set; }
    /// <summary>
    /// Percent (0.0-1.0) of libraries included in Recommended
    /// </summary>
    /// <remarks>Introduced in v0.7.0</remarks>
    public float PercentOfLibrariesIncludedInRecommended { get; set; }
    /// <summary>
    /// Percent (0.0-1.0) of libraries included in Dashboard
    /// </summary>
    /// <remarks>Introduced in v0.7.0</remarks>
    public float PercentOfLibrariesIncludedInDashboard { get; set; }
    /// <summary>
    /// Total reading hours of all users
    /// </summary>
    /// <remarks>Introduced in v0.7.0</remarks>
    public long TotalReadingHours { get; set; }
    /// <summary>
    /// The encoding the server is using to save media
    /// </summary>
    /// <remarks>Added in v0.7.3</remarks>
    public EncodeFormat EncodeMediaAs { get; set; }
    /// <summary>
    /// The last user reading progress on the server (in UTC)
    /// </summary>
    /// <remarks>Added in v0.7.4</remarks>
    public DateTime LastReadTime { get; set; }
}