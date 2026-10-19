using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Attributes;
using KavitaStats.Data;
using KavitaStats.DTOs.V3;
using KavitaStats.Entities;
using KavitaStats.Entities.Enum;
using KavitaStats.Entities.V3;
using KavitaStats.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KavitaStats.Controllers;

/// <summary>
/// Released with Kavita v0.8.5 with a drastic change of what information is collected and how
/// </summary>
[ApiKeyAuthentication]
[Route("api/v3/stats")]
public class StatsV3Controller : BaseApiController
{
    private readonly ILogger<StatsV3Controller> _logger;
    private readonly DataContextV3 _context;
    private readonly DataContext _v2Context;
    private readonly InstallLifecycleService _installLifecycleService;
    // Kavita's v0.1 GitHub release
    private static readonly DateTime KavitaFirstReleaseDate = new(2021, 1, 20);

    public StatsV3Controller(ILogger<StatsV3Controller> logger, DataContextV3 context, DataContext v2Context,
        InstallLifecycleService installLifecycleService)
    {
        _logger = logger;
        _context = context;
        _v2Context = v2Context;
        _installLifecycleService = installLifecycleService;
    }

    [HttpPost("opt-out")]
    public async Task<ActionResult> OptOut([FromQuery] string installId)
    {
        _logger.LogInformation("Stat collection has been requested to be stopped on {InstallId}", installId);

        await _context.ServerStat
            .Where(s => s.InstallId == installId)
            .ExecuteUpdateAsync(s => s.SetProperty(r => r.OptedOut, true));
        await _v2Context.StatRecord
            .Where(r => r.InstallId == installId)
            .ExecuteUpdateAsync(s => s.SetProperty(r => r.OptedOut, true));

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> AddOrUpdateInstance([FromBody] ServerInfoV3Dto dto)
    {
        var existingRecord = await _context.ServerStat
            .Include(s => s.Libraries)
            .Include(s => s.Relationships)
            .Include(s => s.Users)
            .Include(s => s.MediaErrors)
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.InstallId == dto.InstallId);
        
        if (existingRecord != null)
        {
            _context.Entry(existingRecord).State = EntityState.Modified;
        }
        else
        {
            existingRecord = new ServerStat()
            {
                InstallId = dto.InstallId
            };
            _context.ServerStat.Add(existingRecord);
        }

        existingRecord.OptedOut = false;
        existingRecord.Os = dto.Os;
        existingRecord.IsDocker = dto.IsDocker;
        existingRecord.DotnetVersion = dto.DotnetVersion;
        existingRecord.KavitaVersion = dto.KavitaVersion;
        existingRecord.InitialKavitaVersion = dto.InitialKavitaVersion;
        existingRecord.InitialInstallDate =
            dto.InitialInstallDate < KavitaFirstReleaseDate ? null : dto.InitialInstallDate;
        existingRecord.NumOfCores = dto.NumOfCores;
        existingRecord.OsLocale = dto.OsLocale;
        existingRecord.TimeToPingKavitaStatsApi = dto.TimeToPingKavitaStatsApi;
        
        // After Kavita reports enough of these, we can stop recording
        if (dto.TimeToOpeCbzMs != null && dto.TimeToOpenCbzPages != null)
        {
            existingRecord.TimeToOpeCbzMs = dto.TimeToOpeCbzMs.Value;
            existingRecord.TimeToOpenCbzPages = dto.TimeToOpenCbzPages.Value;    
        }

        // Update Media properties
        existingRecord.NumberOfCollections = dto.NumberOfCollections;
        existingRecord.NumberOfReadingLists = dto.NumberOfReadingLists;
        existingRecord.TotalFiles = dto.TotalFiles;
        existingRecord.TotalGenres = dto.TotalGenres;
        existingRecord.TotalSeries = dto.TotalSeries;
        existingRecord.TotalLibraries = dto.TotalLibraries;
        existingRecord.TotalPeople = dto.TotalPeople;
        existingRecord.MaxSeriesInALibrary = dto.MaxSeriesInALibrary;
        existingRecord.MaxVolumesInASeries = dto.MaxVolumesInASeries;
        existingRecord.MaxChaptersInASeries = dto.MaxChaptersInASeries;

        // Update Server properties
        existingRecord.OpdsEnabled = dto.OpdsEnabled;
        existingRecord.EncodeMediaAs = dto.EncodeMediaAs;
        existingRecord.LastReadTime = dto.LastReadTime;
        existingRecord.ActiveKavitaPlusSubscription = dto.ActiveKavitaPlusSubscription;
        existingRecord.UsingRestrictedProfiles = dto.Users.Any(u => u.AgeRestriction.AgeRating != AgeRating.NotApplicable);
        existingRecord.MatchedMetadataEnabled = dto.MatchedMetadataEnabled;
        existingRecord.OidcEnabled = dto.OidcEnabled;
        
        // Update Libraries
        existingRecord.Libraries ??= new List<LibraryStat>();
        existingRecord.Libraries.Clear();
        foreach (var libraryDto in dto.Libraries)
        {
            existingRecord.Libraries.Add(new LibraryStat
            {
                IncludeInDashboard = libraryDto.IncludeInDashboard,
                IncludeInSearch = libraryDto.IncludeInSearch,
                UsingFolderWatching = libraryDto.UsingFolderWatching,
                UsingExcludePatterns = libraryDto.UsingExcludePatterns,
                CreateCollectionsFromMetadata = libraryDto.CreateCollectionsFromMetadata,
                CreateReadingListsFromMetadata = libraryDto.CreateReadingListsFromMetadata,
                LibraryType = libraryDto.LibraryType,
                LastScanned = libraryDto.LastScanned,
                NumberOfFolders = libraryDto.NumberOfFolders,
                FileTypes = libraryDto.FileTypes.ToList(),
                EnabledMetadata = libraryDto.EnabledMetadata,
                FolderWatchMode = libraryDto.FolderWatchMode,
                LastScanDurationMs = libraryDto.LastScanDurationMs,
                LastScanFolderCount = libraryDto.LastScanFolderCount,
                LastScanListMs = libraryDto.LastScanListMs,
                LastScanWalkMs = libraryDto.LastScanWalkMs,
                LastScanParseMs = libraryDto.LastScanParseMs,
                MultiFileEpubChapterCount = libraryDto.MultiFileEpubChapterCount
            });
        }

        existingRecord.TotalMediaErrors = dto.MediaErrors?.Sum(e => e.Count);
        existingRecord.MediaErrors ??= new List<MediaErrorStat>();
        existingRecord.MediaErrors.Clear();
        foreach (var mediaErrorDto in dto.MediaErrors ?? [])
        {
            existingRecord.MediaErrors.Add(new MediaErrorStat
            {
                Reason = mediaErrorDto.Reason,
                Count = mediaErrorDto.Count,
                DismissedCount = mediaErrorDto.DismissedCount
            });
        }
        
        // Update Relationships
        existingRecord.Relationships ??= new List<RelationshipStat>();
        existingRecord.Relationships.Clear();
        foreach (var relationshipDto in dto.Relationships)
        {
            existingRecord.Relationships.Add(new RelationshipStat
            {
                Count = relationshipDto.Count,
                Relationship = relationshipDto.Relationship
            });
        }

        // Update Users
        existingRecord.Users ??= new List<UserStat>();
        existingRecord.Users.Clear();
        foreach (var userDto in dto.Users)
        {
            existingRecord.Users.Add(new UserStat
            {
                AgeRestriction = new UserAgeRestriction
                {
                    AgeRating = userDto.AgeRestriction.AgeRating,
                    IncludeUnknowns = userDto.AgeRestriction.IncludeUnknowns
                },
                LastReadTime = userDto.LastReadTime,
                LastLogin = userDto.LastLogin,
                IsEmailConfirmed = userDto.IsEmailConfirmed,
                HasValidEmail = userDto.HasValidEmail,
                PercentageOfLibrariesHasAccess = userDto.PercentageOfLibrariesHasAccess,
                ReadingListsCreatedCount = userDto.ReadingListsCreatedCount,
                CollectionsCreatedCount = userDto.CollectionsCreatedCount,
                WantToReadSeriesCount = userDto.WantToReadSeriesCount,
                Locale = userDto.Locale,
                ActiveTheme = userDto.ActiveTheme,
                SeriesBookmarksCreatedCount = userDto.SeriesBookmarksCreatedCount,
                HasAniListToken = userDto.HasAniListToken,
                HasMALToken = userDto.HasMALToken,
                SmartFilterCreatedCount = userDto.SmartFilterCreatedCount,
                IsSharingReviews = userDto.IsSharingReviews,
                IsSharingProfile = userDto.IsSharingProfile,
                IsSharingAnnotations = userDto.IsSharingAnnotations,
                DevicePlatforms = userDto.DevicePlatforms.ToList(),
                Roles = userDto.Roles.ToList(),
                IdentityProvider = userDto.IdentityProvider,
                TotalSecondsRead = userDto.TotalSecondsRead,
                TotalPagesRead = userDto.TotalPagesRead,
                TotalWordsRead = userDto.TotalWordsRead,
                UserId = userDto.UserId,
            });
        }

        await _installLifecycleService.RecordReportAsync(existingRecord, dto.KavitaPlusState, dto.KavitaPlusIsFree);

        await _context.SaveChangesAsync();

        return Ok();
    }
}