using System;
using System.Collections.Generic;
using KavitaStats.Entities.Enum;

namespace KavitaStats.Entities.V3;

public class UserStat
{
    public int Id { get; set; }
    public UserAgeRestriction AgeRestriction { get; set; }
    public DateTime LastReadTime { get; set; }
    public DateTime LastLogin { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool HasValidEmail { get; set; }
    public float PercentageOfLibrariesHasAccess { get; set; }
    public int ReadingListsCreatedCount { get; set; }
    public int CollectionsCreatedCount { get; set; }
    public int WantToReadSeriesCount { get; set; }
    public string Locale { get; set; }
    public string ActiveTheme { get; set; }
    public int SeriesBookmarksCreatedCount { get; set; }
    public bool HasAniListToken { get; set; }
    public bool HasMALToken { get; set; }
    public int SmartFilterCreatedCount { get; set; }
    public bool IsSharingReviews { get; set; }
    /// <summary>
    /// Is the user sharing their profile
    /// </summary>
    public bool IsSharingProfile { get; set; }
    /// <summary>
    /// Is the user sharing annotations
    /// </summary>
    public bool IsSharingAnnotations { get; set; }

    public List<DevicePlatform> DevicePlatforms { get; set; } 
    public List<string> Roles { get; set; } 
    
    public IdentityProvider IdentityProvider { get; set; }
    /// <summary>
    /// Total seconds read
    /// </summary>
    /// <remarks>Powers Top Reader badges</remarks>
    public long TotalSecondsRead { get; set; }
    /// <summary>
    /// Total pages read
    /// </summary>
    /// <remarks>Powers Top Reader badges</remarks>
    public long TotalPagesRead { get; set; }
    /// <summary>
    /// Total words read
    /// </summary>
    /// <remarks>Powers Top Reader badges</remarks>
    public long TotalWordsRead { get; set; }
    /// <summary>
    /// An anonymous identifier for the social badges feature. This is the InstallId (which is malleable) and UserId from DB. 
    /// </summary>
    public string? UserId { get; set; }
    
    
    public int ServerStatId { get; set; }
    public virtual ServerStat ServerStat { get; set; }
}