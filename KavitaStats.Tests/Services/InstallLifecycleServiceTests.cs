using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Data;
using KavitaStats.Entities.Enum;
using KavitaStats.Entities.V3;
using KavitaStats.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace KavitaStats.Tests.Services;

public class InstallLifecycleServiceTests : AbstractDbTest
{
    private const string InstallId = "abc123";

    [Fact]
    public async Task ActiveReport_NoPeriod_OpensPeriod()
    {
        var context = await CreateDatabase();

        await Report(context, KavitaPlusState.Active, isFree: true);

        var period = Assert.Single(await GetPeriods(context));
        Assert.Null(period.EndedOn);
        Assert.Null(period.CancellingOn);
        Assert.True(period.IsFree);
        Assert.False(period.Seeded);
    }

    [Fact]
    public async Task CancellingThenActive_ClearsCancellingOn()
    {
        var context = await CreateDatabase();

        await Report(context, KavitaPlusState.Cancelling);
        Assert.NotNull(Assert.Single(await GetPeriods(context)).CancellingOn);

        await Report(context, KavitaPlusState.Active);

        var period = Assert.Single(await GetPeriods(context));
        Assert.Null(period.CancellingOn);
        Assert.Null(period.EndedOn);
    }

    [Fact]
    public async Task ExpiredReport_ClosesWithExpired()
    {
        var context = await CreateDatabase();
        await Report(context, KavitaPlusState.Active);

        await Report(context, KavitaPlusState.Expired);

        var period = Assert.Single(await GetPeriods(context));
        Assert.NotNull(period.EndedOn);
        Assert.Equal(KavitaPlusPeriodEndReason.Expired, period.EndReason);
    }

    [Fact]
    public async Task NoneReport_ClosesWithLicenseRemoved()
    {
        var context = await CreateDatabase();
        await Report(context, KavitaPlusState.Active);

        await Report(context, KavitaPlusState.None);

        var period = Assert.Single(await GetPeriods(context));
        Assert.NotNull(period.EndedOn);
        Assert.Equal(KavitaPlusPeriodEndReason.LicenseRemoved, period.EndReason);
    }

    [Fact]
    public async Task UnreachableState_LeavesOpenPeriod()
    {
        var context = await CreateDatabase();
        await Report(context, KavitaPlusState.Cancelling);
        var before = Assert.Single(await GetPeriods(context));

        await Report(context, KavitaPlusState.Unreachable, activeFlag: false);

        var after = Assert.Single(await GetPeriods(context));
        Assert.Null(after.EndedOn);
        Assert.Equal(before.CancellingOn, after.CancellingOn);
    }

    [Fact]
    public async Task IsFreeChange_ClosesAndOpensSameDay()
    {
        var context = await CreateDatabase();
        await Report(context, KavitaPlusState.Active, isFree: true);

        await Report(context, KavitaPlusState.Active, isFree: false);

        var periods = await GetPeriods(context);
        Assert.Equal(2, periods.Count);
        Assert.True(periods[0].IsFree);
        Assert.Equal(KavitaPlusPeriodEndReason.PlanChanged, periods[0].EndReason);
        Assert.False(periods[1].IsFree);
        Assert.Null(periods[1].EndedOn);
        Assert.Equal(periods[0].EndedOn!.Value.Date, periods[1].StartedOn.Date);
    }

    [Fact]
    public async Task SilentFor30Days_ClosesWithLastReportDate()
    {
        var context = await CreateDatabase();
        var lastSeen = DateTime.UtcNow.AddDays(-31);
        await SeedInstallWithOpenPeriod(context, lastSeen);

        var closed = await new InstallLifecycleService(context, NullLogger<InstallLifecycleService>.Instance)
            .CloseSilentPeriodsAsync();

        Assert.Equal(1, closed);
        var period = Assert.Single(await GetPeriods(context));
        Assert.Equal(lastSeen, period.EndedOn);
        Assert.Equal(KavitaPlusPeriodEndReason.WentSilent, period.EndReason);
    }

    [Fact]
    public async Task SilentAndOptedOut_ClosesWithOptedOut()
    {
        var context = await CreateDatabase();
        await SeedInstallWithOpenPeriod(context, DateTime.UtcNow.AddDays(-31));
        context.ServerStat.Add(new ServerStat { InstallId = InstallId, OptedOut = true });
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        await new InstallLifecycleService(context, NullLogger<InstallLifecycleService>.Instance)
            .CloseSilentPeriodsAsync();

        var period = Assert.Single(await GetPeriods(context));
        Assert.Equal(KavitaPlusPeriodEndReason.OptedOut, period.EndReason);
    }

    [Fact]
    public async Task ReturnAfterWentSilent_ReopensSamePeriod()
    {
        var context = await CreateDatabase();
        await SeedInstallWithOpenPeriod(context, DateTime.UtcNow.AddDays(-31));
        await new InstallLifecycleService(context, NullLogger<InstallLifecycleService>.Instance)
            .CloseSilentPeriodsAsync();
        context.ChangeTracker.Clear();

        await Report(context, KavitaPlusState.Active);

        var period = Assert.Single(await GetPeriods(context));
        Assert.Null(period.EndedOn);
        Assert.Null(period.EndReason);
    }

    [Fact]
    public async Task NullState_FlagFalse_ClosesWithUnknown()
    {
        var context = await CreateDatabase();
        await Report(context, null, activeFlag: true);

        await Report(context, null, activeFlag: false);

        var period = Assert.Single(await GetPeriods(context));
        Assert.NotNull(period.EndedOn);
        Assert.Equal(KavitaPlusPeriodEndReason.Unknown, period.EndReason);
    }

    [Fact]
    public async Task NullState_FlagTrue_OpensPeriod()
    {
        var context = await CreateDatabase();

        await Report(context, null, activeFlag: true);

        var period = Assert.Single(await GetPeriods(context));
        Assert.Null(period.EndedOn);
        Assert.Null(period.IsFree);
    }

    private static async Task Report(DataContextV3 context, KavitaPlusState? state, bool? isFree = null,
        bool? activeFlag = null)
    {
        var install = new ServerStat
        {
            InstallId = InstallId,
            KavitaVersion = "0.8.9.0",
            InitialKavitaVersion = "0.8.1.0",
            TotalSeries = 10,
            ActiveKavitaPlusSubscription = activeFlag ?? state is KavitaPlusState.Active or KavitaPlusState.Cancelling
        };

        await new InstallLifecycleService(context, NullLogger<InstallLifecycleService>.Instance)
            .RecordReportAsync(install, state, isFree);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }

    private static async Task SeedInstallWithOpenPeriod(DataContextV3 context, DateTime lastSeen)
    {
        context.InstallLifecycle.Add(new InstallLifecycle
        {
            InstallId = InstallId,
            FirstSeen = lastSeen.AddDays(-100),
            LastSeen = lastSeen,
            InitialVersion = "0.8.1.0",
            LastVersion = "0.8.9.0",
            KavitaPlusPeriods = [new KavitaPlusPeriod { StartedOn = lastSeen.AddDays(-100) }]
        });
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }

    private static async Task<List<KavitaPlusPeriod>> GetPeriods(DataContextV3 context)
    {
        return await context.KavitaPlusPeriod
            .Where(p => p.InstallLifecycle.InstallId == InstallId)
            .OrderBy(p => p.Id)
            .AsNoTracking()
            .ToListAsync();
    }
}
