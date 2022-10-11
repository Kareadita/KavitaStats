using System;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Attributes;
using KavitaStats.Data;
using KavitaStats.DTOs.V1;
using KavitaStats.Entities;
using KavitaStats.Entities.Enum;
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

        [HttpGet]
        [HttpPost]
        public async Task<ActionResult<V1Response>> AddOrUpdateInstance([FromBody] StatRecordV1Dto dto)
        {
            _logger.LogInformation("[v1] {InstallId} is using v1 interface. Version: {Version}", dto.InstallId, dto.ServerInfo.KavitaVersion);
            
            var existingRecord =
                await _context.StatRecord.Where(r => r.InstallId == dto.InstallId).SingleOrDefaultAsync();

            if (existingRecord != null)
            {
                // perform update
                existingRecord.DotnetVersion = dto.ServerInfo.DotNetVersion;
                existingRecord.IsDocker = dto.ServerInfo.IsDocker;
                existingRecord.KavitaVersion = dto.ServerInfo.KavitaVersion;
                existingRecord.NumOfCores = dto.ServerInfo.NumOfCores;
                existingRecord.LastUpdated = DateTime.Now;
                existingRecord.HasBookmarks = false;
                existingRecord.NumberOfLibraries = 0;
                existingRecord.ActiveSiteTheme = null;
                existingRecord.MangaReaderMode = ReaderMode.LeftRight;
                existingRecord.NumberOfCollections = 0;
                existingRecord.NumberOfUsers = 0;
                existingRecord.NumberOfReadingLists = 0;
                existingRecord.TotalFiles = 0;
                existingRecord.OPDSEnabled = false;
            }
            else
            {
                await _context.StatRecord.AddAsync(new StatRecord()
                {
                    InstallId = dto.InstallId,
                    DotnetVersion = dto.ServerInfo.DotNetVersion,
                    IsDocker = dto.ServerInfo.IsDocker,
                    KavitaVersion = dto.ServerInfo.KavitaVersion,
                    NumOfCores = dto.ServerInfo.NumOfCores,
                    LastUpdated = DateTime.Now,
                    HasBookmarks = false,
                    NumberOfLibraries = 0,
                    ActiveSiteTheme = null,
                    MangaReaderMode = ReaderMode.LeftRight,
                    NumberOfCollections = 0,
                    NumberOfUsers = 0,
                    NumberOfReadingLists = 0,
                    TotalFiles = 0,
                    OPDSEnabled = false,
                    UsingRestrictedProfiles = false,
                    OptedOut = false
                });
                _logger.LogInformation("New install on v1 api");
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