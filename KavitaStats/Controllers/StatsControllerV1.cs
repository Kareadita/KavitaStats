using System;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Attributes;
using KavitaStats.Data;
using KavitaStats.DTOs.V1;
using KavitaStats.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KavitaStats.Controllers
{
    [ApiKeyAuthentication]
    [Route("api/InstallationStats")]
    public class StatsControllerV1 : BaseApiController
    {
        private readonly ILogger<StatsControllerV1> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataContext _context;

        public StatsControllerV1(ILogger<StatsControllerV1> logger, IUnitOfWork unitOfWork, 
            DataContext context)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<V1Response>> AddOrUpdateInstance([FromBody] StatRecordV1Dto dto)
        {
            var existingRecord =
                await _context.StatRecord.Where(r => r.InstallId == dto.InstallId).SingleOrDefaultAsync();

            if (existingRecord != null)
            {
                // perform update
                existingRecord.DotnetVersion = dto.ServerInfo.DotNetVersion;
                existingRecord.IsDocker = dto.ServerInfo.IsDocker;
                existingRecord.KavitaVersion = dto.ServerInfo.KavitaVersion;
                existingRecord.LastUpdated = DateTime.Now;
            }
            else
            {
                await _context.StatRecord.AddAsync(new StatRecord()
                {
                    InstallId = dto.InstallId,
                    DotnetVersion = dto.ServerInfo.DotNetVersion,
                    IsDocker = dto.ServerInfo.IsDocker,
                    KavitaVersion = dto.ServerInfo.KavitaVersion,
                    LastUpdated = DateTime.Now
                });
            }

            if (!_unitOfWork.HasChanges()) return Ok(new V1Response
            {
                Success = true
            });
            
            if (await _unitOfWork.CommitAsync())
            {
                return Ok(new V1Response
                {
                    Success = true
                });
            }
            
            return BadRequest(new V1Response
            {
                Success = true,
                Error = "There was an issue updating KavitaStats"
            });
        }
    }
}