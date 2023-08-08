using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Data;
using KavitaStats.DTOs;
using KavitaStats.DTOs.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Controllers;

public class UiController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DataContext _dataContext;

    public UiController(IUnitOfWork unitOfWork, DataContext dataContext)
    {
        _unitOfWork = unitOfWork;
        _dataContext = dataContext;
    }

    [HttpGet("total-users")]
    public async Task<ActionResult<int>> GetTotalUserCount()
    {
        return await _dataContext.StatRecord
            .Select(s => s.InstallId)
            .Distinct()
            .AsNoTracking()
            .CountAsync();
        
        // select count((select distinct installId from StatRecord)) from StatRecord;
    }

    [HttpGet("volumes-in-a-series")]
    public async Task<ActionResult<VolumesInASeriesDto>> GetVolumesInASeries()
    {
        // select min(MaxVolumesInASeries), max(MaxVolumesInASeries), avg(MaxVolumesInASeries) from StatRecord;
        var ret = new VolumesInASeriesDto()
        {
            Maximum = await _dataContext.StatRecord.Select(s => s.MaxVolumesInASeries).MaxAsync(),
            Minimum = await _dataContext.StatRecord.Select(s => s.MaxVolumesInASeries).MinAsync(),
            Average = await _dataContext.StatRecord.Select(s => s.MaxVolumesInASeries).AverageAsync(),
        };

        return ret;

    }

    [HttpGet("installs-by-release")]
    public async Task<ActionResult<IEnumerable<ReleaseInstallCountDto>>> GetUsersByRelease(int cutoffDays = 0)
    {
        var distinctInstalls =  await _dataContext.StatRecord
            .Select(s => s.KavitaVersion)
            .Distinct()
            .OrderByDescending(r => r)
            .AsNoTracking()
            .ToListAsync();

        var releaseInstalls = new List<ReleaseInstallCountDto>();
        foreach (var install in distinctInstalls)
        {

            // var t = await _dataContext.StatRecord.Select(sr => new
            // {
            //     InstallCount = _dataContext.StatRecord.CountAsync(s => s.KavitaVersion == install),
            //     DockerCount = _dataContext.StatRecord.Where(s => s.IsDocker).CountAsync(s => s.KavitaVersion == install),
            // })

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
    
    [HttpGet("shield-badge")]
    public async Task<ActionResult<ShieldBadgeDto>> GetActiveInstalls()
    {
        return Ok(new ShieldBadgeDto()
        {
            Message = "Active Installs: " + FormatNumberCompact(await _dataContext.StatRecord.CountAsync())
        });
    }
    
    private static string FormatNumberCompact(long number)
    {
        return number switch
        {
            // If greater than or equal to a million
            >= 1000000 => (number / 1000000.0).ToString("0.#") + "M",
            // If greater than or equal to a thousand
            >= 1000 => (number / 1000.0).ToString("0.#") + "K",
            _ => number.ToString()
        };
    }
    
    // I need install growth over time (this is by created date vs install version)
    // Pie graph of Installs vs OS/Version/ 
}