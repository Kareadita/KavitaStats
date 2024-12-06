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

    public ICollection<UserStatDevicePlatform> DevicePlatforms { get; set; } 
    public ICollection<UserStatRole> Roles { get; set; } 
    
    public int ServerStatId { get; set; }
    public virtual ServerStat ServerStat { get; set; }
}