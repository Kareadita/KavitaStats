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

        [HttpGet]
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