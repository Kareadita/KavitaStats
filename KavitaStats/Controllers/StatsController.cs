using System;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Attributes;
using KavitaStats.Data;
using KavitaStats.DTOs;
using KavitaStats.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KavitaStats.Controllers
{
    [ApiKeyAuthentication]
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

        [HttpPost]
        public async Task<ActionResult> AddOrUpdateInstance([FromBody] StatRecordDto dto)
        {
            var existingRecord =
                await _context.StatRecord.Where(r => r.InstallId == dto.InstallId).SingleOrDefaultAsync();

            if (existingRecord != null)
            {
                // perform update
                existingRecord.DotnetVersion = dto.DotnetVersion;
                existingRecord.IsDocker = dto.IsDocker;
                existingRecord.KavitaVersion = dto.KavitaVersion;
            }
            else
            {
                await _context.StatRecord.AddAsync(new StatRecord()
                {
                    InstallId = dto.InstallId,
                    DotnetVersion = dto.DotnetVersion,
                    IsDocker = dto.IsDocker,
                    KavitaVersion = dto.KavitaVersion
                });
            }

            if (!_unitOfWork.HasChanges()) return Ok();
            if (await _unitOfWork.CommitAsync())
            {
                return Ok();
            }
            
            return BadRequest("There was an issue updating KavitaStats");
        }
    }
}