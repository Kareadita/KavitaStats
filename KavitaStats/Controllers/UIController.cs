using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Constants;
using KavitaStats.Data;
using KavitaStats.DTOs;
using KavitaStats.DTOs.UI;
using KavitaStats.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Controllers;

[EnableCors("Public")]
[EnableRateLimiting("ui")]
public class UiController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly DataContextV3 _dataContextV3;
    private readonly IUiStatsCacheService _cacheService;

    public UiController(DataContext dataContext, DataContextV3 dataContextV3, IUiStatsCacheService cacheService)
    {
        _dataContext = dataContext;
        _dataContextV3 = dataContextV3;
        _cacheService = cacheService;
    }

    [HttpGet("total-users")]
    [OutputCache(PolicyName = CacheConstants.TenMinutes)]
    public async Task<ActionResult<int>> GetTotalUserCount()
    {
        return Ok(await _cacheService.GetActiveInstallsAsync());
    }

    /// <summary>
    /// For a given theme, return the number of active users with theme active
    /// </summary>
    /// <remarks>This is used on ThemeRepo</remarks>
    /// <param name="theme"></param>
    /// <returns></returns>
    [HttpGet("theme-users")]
    [OutputCache(PolicyName = CacheConstants.TenMinutes)]
    public async Task<ActionResult<ShieldBadgeDto>> GetUsersByTheme(string theme)
    {
        var count = await _dataContextV3.UserStat.Where(u => u.ActiveTheme == theme)
            .CountAsync();

        return Ok(new ShieldBadgeDto()
        {
            Label = "Active",
            Message = FormatNumberCompact(count)
        });
    }

    [HttpGet("installs-by-release")]
    [OutputCache(PolicyName = CacheConstants.TenMinutes)]
    public async Task<ActionResult<IEnumerable<ReleaseInstallCountDto>>> GetUsersByRelease(int cutoffDays = 0)
    {
        var distinctInstalls = await _dataContext.StatRecord
            .Select(s => s.KavitaVersion)
            .Distinct()
            .OrderByDescending(r => r)
            .AsNoTracking()
            .ToListAsync();

        var releaseInstalls = new List<ReleaseInstallCountDto>();
        foreach (var install in distinctInstalls)
        {
            var cuttoffDate = DateTime.Now - TimeSpan.FromDays(cutoffDays);

            var count = await _dataContext.StatRecord.CountAsync(s =>
                s.KavitaVersion == install || (cutoffDays > 0 && s.LastUpdated >= cuttoffDate));
            if (count == 0) continue;

            releaseInstalls.Add(new ReleaseInstallCountDto()
            {
                InstallCount = count,
                ReleaseVersion = install
            });
        }

        // TODO: Need to order by Version number .OrderBy(r => new Version(r.ReleaseVersion))
        return releaseInstalls;
    }

    /// <summary>
    /// Generates the shield.io status badge for Kavita's readme
    /// </summary>
    /// <returns></returns>
    [HttpGet("shield-badge")]
    [OutputCache(PolicyName = CacheConstants.TenMinutes)]
    public async Task<ActionResult<ShieldBadgeDto>> GetServerBadge()
    {
        return Ok(new ShieldBadgeDto()
        {
            Message = FormatNumberCompact(await _cacheService.GetTotalInstallsAsync())
        });
    }

    private static string FormatNumberCompact(long number)
    {
        return number switch
        {
            >= 1000000 => (number / 1000000.0).ToString("0.#") + "M",
            >= 1000 => (number / 1000.0).ToString("0.#") + "K",
            _ => number.ToString()
        };
    }
}
