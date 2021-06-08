using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.UsageStatistics;
using Api.Domain.UsageStatistics.Contracts;
using Api.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsageStatisticsController : ControllerBase
    {
        private readonly ILogger<UsageStatisticsController> _logger;
        private readonly IUsageStatisticsRepository _statisticsRepository;

        public UsageStatisticsController(ILogger<UsageStatisticsController> logger,
            IUsageStatisticsRepository statisticsRepository)
        {
            _logger = logger;
            _statisticsRepository = statisticsRepository;
        }

        //discuss the best moment for request this endpoint
        //maybe keep the statistics in cache/temp file and send it once a day then remove
        [HttpPost]
        public async Task<IActionResult> SaveStatistics([FromBody] UsageStatistics usageStatistics)
        {
            await _statisticsRepository.Add(usageStatistics);

            return Success();
        }

        //discuss the best moment for request this endpoint
        //maybe each access, maybe only in the first access
        [HttpPost("client-stats")]
        public async Task<IActionResult> SaveClientStatistics([FromBody] ClientInfo info)
        {
            await _statisticsRepository.Add(info);

            return Success();
        }

        //discuss the best moment for request this endpoint
        //maybe each server startup, maybe only in the first run
        [HttpPost("server-stats")]
        public async Task<IActionResult> SaveServerStatistics([FromBody] ServerInfo info)
        {
            await _statisticsRepository.Add(info);

            return Success();
        }
        //
        // //discuss the best moment for request this endpoint
        // //maybe each new lib creation, maybe once a day
        // [HttpPost("library-stats")]
        // public async Task<IActionResult> SaveLibraryStatistics([FromBody] LibraryType info)
        // {
        //     await _statisticsRepository.Add(info);
        //
        //     return Success();
        // }
        //
        // //discuss the best moment for request this endpoint
        // //maybe each scan, maybe once a day
        // [HttpPost("file-type-stats")]
        // public async Task<IActionResult> SaveFileTypeStatistics([FromBody] IEnumerable<string> fileTypes)
        // {
        //     await _statisticsRepository.Add(info);
        //
        //     return Success();
        // }

        protected IActionResult Success()
        {
            return Ok(new ApiResponse {Success = true});
        }
    }
}