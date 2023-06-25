using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Attributes;
using KavitaStats.Data;
using KavitaStats.DTOs;
using KavitaStats.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Serilog.Context.LogContext;

namespace KavitaStats.Controllers;

[ApiKeyAuthentication]
[Route("api/v2/[controller]")]
public class StatsController : BaseApiController
{
    private readonly ILogger<StatsController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DataContext _context;

    public StatsController(ILogger<StatsController> logger, IUnitOfWork unitOfWork, DataContext context)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _context = context;
    }

    /// <summary>
    /// Validates an install ID as valid or not.
    /// </summary>
    /// <remarks>This is used by KavitaEmail service to validate valid installs are using it</remarks>
    /// <param name="installId"></param>
    /// <returns></returns>
    [HttpGet("validate")]
    public async Task<ActionResult<bool>> ValidateInstallId(string installId)
    {
        return Ok(await _context.StatRecord.AnyAsync(r => r.InstallId.Equals(installId)));
    }

    [HttpPost("out-out")]
    public async Task<ActionResult> UpdateAsCancelled([FromQuery] string installId)
    {
        _logger.LogInformation("Stat collection has been requested to be stopped on {InstallId}", installId);
        var existingRecord =
            await _context.StatRecord.Where(r => r.InstallId == installId).FirstOrDefaultAsync();
        if (existingRecord == null) return Ok();

        existingRecord.OptedOut = true;

        return Ok();
    }
        
    [HttpPost]
    public async Task<ActionResult> AddOrUpdateInstance([FromBody] StatRecordDto dto)
    {
        _logger.LogInformation("Handling Update for InstallId: {InstallId}", dto.InstallId);
        _logger.LogDebug("Payload: {@Dto}", dto);
        try
        {
            var existingRecord =
                await _context.StatRecord.Where(r => r.InstallId == dto.InstallId).FirstOrDefaultAsync();

            // This is not architected correctly and has issues. This needs junction table instead
            // var colors = await ProcessMangaReaderBackgroundColors(dto);
            // var pageSplittingModes = await ProcessMangaReaderPageSplittingModes(dto);
            // var mangaReaderLayoutModes = await ProcessMangaReaderLayoutModes(dto);            
            var colors = new List<Color>();
            var pageSplittingModes = new List<PageSplit>();
            var mangaReaderLayoutModes = new List<MangaReaderLayoutMode>();
            //var fileFormats = await ProcessFileFormats(dto);


            if (existingRecord != null)
            {
                // perform update
                existingRecord.DotnetVersion = dto.DotnetVersion;
                existingRecord.IsDocker = dto.IsDocker;
                existingRecord.KavitaVersion = dto.KavitaVersion;
                existingRecord.NumOfCores = dto.NumOfCores;
                existingRecord.LastUpdated = DateTime.Now;
                existingRecord.HasBookmarks = dto.HasBookmarks;
                existingRecord.NumberOfLibraries = dto.NumberOfLibraries;
                existingRecord.ActiveSiteTheme = dto.ActiveSiteTheme;
                existingRecord.MangaReaderMode = dto.MangaReaderMode;
                existingRecord.NumberOfCollections = dto.NumberOfCollections;
                existingRecord.NumberOfUsers = dto.NumberOfUsers;
                existingRecord.NumberOfReadingLists = dto.NumberOfReadingLists;
                existingRecord.TotalFiles = dto.TotalFiles;
                existingRecord.OPDSEnabled = dto.OPDSEnabled;
                existingRecord.TotalGenres = dto.TotalGenres;
                existingRecord.TotalPeople = dto.TotalPeople;
                existingRecord.UsersOnCardLayout = dto.UsersOnCardLayout;
                existingRecord.UsersOnListLayout = dto.UsersOnListLayout;
                existingRecord.MaxSeriesInALibrary = dto.MaxSeriesInALibrary;
                existingRecord.MaxVolumesInASeries = dto.MaxVolumesInASeries;
                existingRecord.MaxChaptersInASeries = dto.MaxChaptersInASeries;
                existingRecord.UsingSeriesRelationships = dto.UsingSeriesRelationships;
                existingRecord.OptedOut = false;
                existingRecord.UsingRestrictedProfiles = dto.UsingRestrictedProfiles;
                existingRecord.TotalReadingHours = dto.TotalReadingHours;
                existingRecord.PercentOfLibrariesIncludedInDashboard = dto.PercentOfLibrariesIncludedInDashboard;
                existingRecord.PercentOfLibrariesIncludedInRecommended = dto.PercentOfLibrariesIncludedInRecommended;
                existingRecord.PercentOfLibrariesIncludedInSearch = dto.PercentOfLibrariesIncludedInSearch;
                existingRecord.PercentOfLibrariesWithFolderWatchingEnabled =
                    dto.PercentOfLibrariesWithFolderWatchingEnabled;
                existingRecord.UsersWithEmulateComicBook = dto.UsersWithEmulateComicBook;
                existingRecord.EncodeMediaAs = dto.EncodeMediaAs;
                existingRecord.LastReadTime = dto.LastReadTime;
                    
                    
                existingRecord.MangaReaderBackgroundColors = colors;
                //_unitOfWork.ColorRepository.Delete(existingRecord.MangaReaderBackgroundColors.Where(c => !colors.Select(c2 => c2.Value).Contains(c.Value)));
                    
                existingRecord.MangaReaderPageSplittingModes = pageSplittingModes;   
                //_unitOfWork.PageSplitRepository.Delete(existingRecord.MangaReaderPageSplittingModes.Where(c => !pageSplittingModes.Select(c2 => c2.PageSplitOption).Contains(c.PageSplitOption)));
                    
                existingRecord.MangaReaderLayoutModes = mangaReaderLayoutModes;
                //_unitOfWork.MangaReaderLayoutModeRepository.Delete(existingRecord.MangaReaderLayoutModes.Where(c => !mangaReaderLayoutModes.Select(c2 => c2.ReaderMode).Contains(c.ReaderMode)));
                    
                existingRecord.FileFormats = new List<FileFormat>();
                // _unitOfWork.FileFormatRepository.Delete(existingRecord.FileFormats
                //     .Where(c => !fileFormats.Select(c2 => c2.Extension).Contains(c.Extension)));
            }
            else
            {
                await _context.StatRecord.AddAsync(new StatRecord()
                {
                    InstallId = dto.InstallId,
                    DotnetVersion = dto.DotnetVersion,
                    IsDocker = dto.IsDocker,
                    KavitaVersion = dto.KavitaVersion,
                    NumOfCores = dto.NumOfCores,
                    LastUpdated = DateTime.Now,
                    HasBookmarks = dto.HasBookmarks,
                    NumberOfLibraries = dto.NumberOfLibraries,
                    ActiveSiteTheme = dto.ActiveSiteTheme,
                    MangaReaderMode = dto.MangaReaderMode,
                    NumberOfCollections = dto.NumberOfCollections,
                    NumberOfUsers = dto.NumberOfUsers,
                    NumberOfReadingLists = dto.NumberOfReadingLists,
                    TotalFiles = dto.TotalFiles,
                    OPDSEnabled = dto.OPDSEnabled,
                    TotalGenres = dto.TotalGenres,
                    TotalPeople = dto.TotalPeople,
                    UsersOnCardLayout = dto.UsersOnCardLayout,
                    UsersOnListLayout = dto.UsersOnListLayout,
                    MaxSeriesInALibrary = dto.MaxSeriesInALibrary,
                    MaxVolumesInASeries = dto.MaxVolumesInASeries,
                    MaxChaptersInASeries = dto.MaxChaptersInASeries,
                    UsingSeriesRelationships = dto.UsingSeriesRelationships,
                    OptedOut = false,
                    MangaReaderBackgroundColors = colors,
                    MangaReaderPageSplittingModes = pageSplittingModes,
                    MangaReaderLayoutModes = mangaReaderLayoutModes,
                    FileFormats = new List<FileFormat>(),
                    UsingRestrictedProfiles = dto.UsingRestrictedProfiles,
                    TotalReadingHours = dto.TotalReadingHours,
                    PercentOfLibrariesIncludedInDashboard = dto.PercentOfLibrariesIncludedInDashboard,
                    PercentOfLibrariesIncludedInRecommended = dto.PercentOfLibrariesIncludedInRecommended,
                    PercentOfLibrariesIncludedInSearch = dto.PercentOfLibrariesIncludedInSearch,
                    PercentOfLibrariesWithFolderWatchingEnabled =
                        dto.PercentOfLibrariesWithFolderWatchingEnabled,
                    UsersWithEmulateComicBook = dto.UsersWithEmulateComicBook,
                    EncodeMediaAs = dto.EncodeMediaAs,
                    LastReadTime = dto.LastReadTime
                });
            }

            if (!_unitOfWork.HasChanges()) return Ok();
            if (await _unitOfWork.CommitAsync())
            {
                _logger.LogDebug("{InstallId} updated completely", dto.InstallId);
                 
                return Ok();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "There was an exception when updating v2 install: {InstallId} with {@Dto}", dto.InstallId, dto);
        }
            
        return BadRequest("There was an issue updating KavitaStats");
    }

    private async Task<List<Color>> ProcessMangaReaderBackgroundColors(StatRecordDto dto)
    {
        var colors = new List<Color>();
        if (dto.MangaReaderBackgroundColors == null || dto.MangaReaderBackgroundColors.Count == 0) return colors;
            
        var existingColors = (await _unitOfWork.ColorRepository.FindAll()).ToList();
        foreach (var color in dto.MangaReaderBackgroundColors)
        {
            var existingColor = existingColors.FirstOrDefault(c => c.Value.Equals(color));
            if (existingColor == null)
            {
                existingColor = new Color()
                {
                    Value = color
                };
                _unitOfWork.ColorRepository.Attach(existingColor);
            }

            colors.Add(existingColor);
        }

        return colors;
    }
    private async Task<List<PageSplit>> ProcessMangaReaderPageSplittingModes(StatRecordDto dto)
    {
        var modes = new List<PageSplit>();
        if (dto.MangaReaderPageSplittingModes == null || dto.MangaReaderPageSplittingModes.Count == 0) return modes;
            
        var existingModes = (await _unitOfWork.PageSplitRepository.FindAll()).ToList();
        foreach (var mode in dto.MangaReaderPageSplittingModes)
        {
            var existingMode = existingModes.FirstOrDefault(c => c.PageSplitOption.Equals(mode));
            if (existingMode == null)
            {
                existingMode = new PageSplit()
                {
                    PageSplitOption = mode
                };
                _unitOfWork.PageSplitRepository.Attach(existingMode);
            }

            modes.Add(existingMode);
        }

        return modes;
    }
        
    private async Task<List<MangaReaderLayoutMode>> ProcessMangaReaderLayoutModes(StatRecordDto dto)
    {
        var modes = new List<MangaReaderLayoutMode>();
        if (dto.MangaReaderLayoutModes == null || dto.MangaReaderLayoutModes.Count == 0) return modes;
            
        var existingModes = (await _unitOfWork.MangaReaderLayoutModeRepository.FindAll()).ToList();
        foreach (var mode in dto.MangaReaderLayoutModes)
        {
            var existingMode = existingModes.FirstOrDefault(c => c.ReaderMode.Equals(mode));
            if (existingMode == null)
            {
                existingMode = new MangaReaderLayoutMode()
                {
                    ReaderMode = mode
                };
                _unitOfWork.MangaReaderLayoutModeRepository.Attach(existingMode);
            }

            modes.Add(existingMode);
        }

        return modes;
    }
    private async Task<List<FileFormat>> ProcessFileFormats(StatRecordDto dto)
    {
        var formats = new List<FileFormat>();
        if (dto.FileFormats == null || dto.FileFormats.Count == 0) return formats;
            
        var existingFormats = (await _unitOfWork.FileFormatRepository.FindAll()).ToList();
        foreach (var fileFormat in dto.FileFormats)
        {
            var existingFormat = existingFormats.FirstOrDefault(c => 
                c.Extension.Equals(fileFormat.Extension) && c.Format == fileFormat.Format);
            if (existingFormat == null)
            {
                existingFormat = new FileFormat()
                {
                    Format = fileFormat.Format,
                    Extension = fileFormat.Extension
                };
                _unitOfWork.FileFormatRepository.Attach(existingFormat);
            }

            formats.Add(existingFormat);
        }

        return formats;
    }
}