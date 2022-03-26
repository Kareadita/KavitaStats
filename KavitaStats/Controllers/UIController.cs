using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Data;
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
        return await _dataContext.StatRecord.CountAsync();
    }
    
    [HttpGet("installs-by-release")]
    public async Task<ActionResult<IEnumerable<ReleaseInstallCountDto>>> GetUsersByRelease()
    {
        var distinctInstalls =  await _dataContext.StatRecord
            .Select(s => s.KavitaVersion)
            .Distinct()
            .AsNoTracking()
            .ToListAsync();

        var releaseInstalls = new List<ReleaseInstallCountDto>();
        foreach (var install in distinctInstalls)
        {
            var installCount = await _dataContext.StatRecord.CountAsync(s => s.KavitaVersion == install);
            
            releaseInstalls.Add(new ReleaseInstallCountDto()
            {
                InstallCount = await _dataContext.StatRecord.CountAsync(s => s.KavitaVersion == install),
                ReleaseVersion = install
            });
        }

        // TODO: Need to order by Version number .OrderBy(r => new Version(r.ReleaseVersion))
        return releaseInstalls;
    }
    
    // I need install growth over time (this is by created date vs install version)
    // Pie graph of Installs vs OS/Version/ 
}