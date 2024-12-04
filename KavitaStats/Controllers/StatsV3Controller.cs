using System.Threading.Tasks;
using KavitaStats.Attributes;
using KavitaStats.Data;
using KavitaStats.DTOs.V2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KavitaStats.Controllers;

/// <summary>
/// Released with Kavita v0.8.5 with a drastic change of what information is collected and how
/// </summary>
[ApiKeyAuthentication]
[Route("api/v3/[controller]")]
public class StatsV3Controller : BaseApiController
{
    private readonly ILogger<StatsV3Controller> _logger;
    private readonly DataContext _context;

    public StatsV3Controller(ILogger<StatsV3Controller> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    // [HttpPost("opt-out")]
    // public async Task<ActionResult> UpdateAsCancelled([FromQuery] string installId)
    // {
    //     _logger.LogInformation("Stat collection has been requested to be stopped on {InstallId}", installId);
    //     
    //     // TODO
    //     // var existingRecord = await _context.Servers.Where(r => r.InstallId == installId).FirstOrDefaultAsync();
    //     // if (existingRecord == null) return Ok();
    //     //
    //     // existingRecord.OptedOut = true;
    //
    //     return Ok();
    // }
    //
    // [HttpPost]
    // public async Task<ActionResult> AddOrUpdateInstance([FromBody] Statv3Dto dto)
    // {
    //     return Ok();
    // }
}