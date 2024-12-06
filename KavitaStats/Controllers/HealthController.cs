using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Controllers;

public class HealthController : BaseApiController
{
    private readonly DataContextV3 _context;

    public HealthController(DataContextV3 context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var sw = Stopwatch.StartNew();
        await _context.ServerStat.FirstOrDefaultAsync();
        
        return Ok(new {DbTime = sw.Elapsed.TotalMilliseconds});
    }
    
}