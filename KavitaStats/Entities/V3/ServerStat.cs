﻿using System;
using System.Collections.Generic;
using KavitaStats.Entities.Enum;
using KavitaStats.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Entities.V3;

[Index("InstallId", IsUnique = true)]
public class ServerStat : IHasDate
{
    public int Id { get; set; }
    public string InstallId { get; set; }
    /// <summary>
    /// Has the server opted out of sending stats
    /// </summary>
    public bool OptedOut { get; set; }
    public string Os { get; set; }
    public bool IsDocker { get; set; }
    public string DotnetVersion { get; set; }
    public string KavitaVersion { get; set; }
    public string InitialKavitaVersion { get; set; }
    public DateTime InitialInstallDate { get; set; }
    public int NumOfCores { get; set; }
    public string OsLocale { get; set; }
    public long TimeToOpeCbzMs { get; set; }
    public long TimeToOpenCbzPages { get; set; }
    public long TimeToPingKavitaStatsApi { get; set; }

    public int NumberOfCollections { get; set; }
    public int NumberOfReadingLists { get; set; }
    public int TotalFiles { get; set; }
    public int TotalGenres { get; set; }
    public int TotalSeries { get; set; }
    public int TotalLibraries { get; set; }
    public int TotalPeople { get; set; }
    public int MaxSeriesInALibrary { get; set; }
    public int MaxVolumesInASeries { get; set; }
    public int MaxChaptersInASeries { get; set; }

    public bool OpdsEnabled { get; set; }
    public EncodeFormat EncodeMediaAs { get; set; }
    public DateTime LastReadTime { get; set; }
    public bool ActiveKavitaPlusSubscription { get; set; }
    public bool UsingRestrictedProfiles { get; set; }
    public bool MatchedMetadataEnabled { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }

    public ICollection<LibraryStat> Libraries { get; set; }
    public ICollection<RelationshipStat> Relationships { get; set; }
    public ICollection<UserStat> Users { get; set; }
    
}