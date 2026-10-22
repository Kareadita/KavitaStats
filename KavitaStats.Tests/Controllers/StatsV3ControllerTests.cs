using System.Threading.Tasks;
using KavitaStats.Controllers;
using KavitaStats.Data;
using KavitaStats.DTOs.V3;
using KavitaStats.Entities;
using KavitaStats.Entities.Enum;
using KavitaStats.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace KavitaStats.Tests.Controllers;

public class StatsV3ControllerTests : AbstractDbTest
{
    private const string InstallId = "abc123";

    private static StatsV3Controller CreateController(DataContextV3 context)
    {
        return new StatsV3Controller(NullLogger<StatsV3Controller>.Instance, context, null!,
            new InstallLifecycleService(context, NullLogger<InstallLifecycleService>.Instance));
    }

    private static ServerInfoV3Dto Payload()
    {
        return new ServerInfoV3Dto
        {
            InstallId = InstallId,
            Os = "Linux",
            DotnetVersion = "10.0.3",
            KavitaVersion = "0.9.2.0",
            InitialKavitaVersion = "0.9.0.0",
            OsLocale = "English",
            Libraries =
            [
                new LibraryStatV3 { FileTypes = [], LibraryType = LibraryType.Book }
            ],
            Relationships = [],
            Users =
            [
                new UserStatV3
                {
                    AgeRestriction = new AgeRestriction { AgeRating = AgeRating.NotApplicable },
                    Locale = "en",
                    ActiveTheme = "Dark",
                    DevicePlatforms = [],
                    Roles = []
                }
            ]
        };
    }

    [Fact]
    public async Task SaveStats_OldPayload_LeavesNewFieldsNull()
    {
        var context = await CreateDatabase();

        await CreateController(context).AddOrUpdateInstance(Payload());

        context.ChangeTracker.Clear();
        var server = await context.ServerStat
            .Include(s => s.Libraries)
            .Include(s => s.MediaErrors)
            .SingleAsync(s => s.InstallId == InstallId);
        var stored = Assert.Single(server.Libraries);
        Assert.Null(stored.LastScanDurationMs);
        Assert.Null(stored.MultiFileEpubChapterCount);
        Assert.Null(stored.FolderWatchMode);
        Assert.Null(stored.FolderWatchMethod);
        Assert.Null(server.EnableFolderWatching);
        Assert.Null(server.TotalMediaErrors);
        Assert.Empty(server.MediaErrors);
        Assert.False(server.UsingRestrictedProfiles);

        var dto = Payload();
        dto.MediaErrors = [];
        await CreateController(context).AddOrUpdateInstance(dto);

        context.ChangeTracker.Clear();
        Assert.Equal(0, (await context.ServerStat.SingleAsync(s => s.InstallId == InstallId)).TotalMediaErrors);
    }
}
