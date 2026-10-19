using System;
using System.Linq;
using System.Threading.Tasks;
using KavitaStats.Data;
using KavitaStats.Entities.Enum;
using KavitaStats.Entities.V3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KavitaStats.Services;

public class InstallLifecycleService(DataContextV3 context, ILogger<InstallLifecycleService> logger)
{
    private static readonly TimeSpan SilentPeriodCutoff = TimeSpan.FromDays(30);

    private readonly record struct ReportedKavitaPlus(bool HasAccess, bool IsCancelling,
        KavitaPlusPeriodEndReason? EndReason);

    /// <summary>
    /// Updates the install's lifecycle row and Kavita+ periods. Does not save
    /// </summary>
    public async Task RecordReportAsync(ServerStat install, KavitaPlusState? kavitaPlusState, bool? kavitaPlusIsFree)
    {
        var now = DateTime.UtcNow;

        var lifecycle = await context.InstallLifecycle
            .Include(l => l.KavitaPlusPeriods)
            .FirstOrDefaultAsync(l => l.InstallId == install.InstallId);

        if (lifecycle == null)
        {
            lifecycle = new InstallLifecycle
            {
                InstallId = install.InstallId,
                FirstSeen = now
            };
            context.InstallLifecycle.Add(lifecycle);
        }

        lifecycle.LastSeen = now;
        lifecycle.InitialVersion = install.InitialKavitaVersion;
        lifecycle.LastVersion = install.KavitaVersion;
        lifecycle.InitialInstallDate = install.InitialInstallDate;
        lifecycle.EverHadSeries |= install.TotalSeries > 0;

        var reported = ToReportedKavitaPlus(kavitaPlusState, install.ActiveKavitaPlusSubscription);
        if (reported == null) return;

        ApplyKavitaPlusReport(lifecycle, reported.Value, kavitaPlusIsFree, now);
    }

    /// <returns>Number of periods closed</returns>
    public async Task<int> CloseSilentPeriodsAsync()
    {
        var cutoff = DateTime.UtcNow - SilentPeriodCutoff;

        var silentPeriods = await context.KavitaPlusPeriod
            .Include(p => p.InstallLifecycle)
            .Where(p => p.EndedOn == null && p.InstallLifecycle.LastSeen < cutoff)
            .ToListAsync();

        if (silentPeriods.Count == 0) return 0;

        var installIds = silentPeriods.Select(p => p.InstallLifecycle.InstallId).ToList();
        var optedOutInstallIds = (await context.ServerStat
            .Where(s => s.OptedOut && installIds.Contains(s.InstallId))
            .Select(s => s.InstallId)
            .ToListAsync())
            .ToHashSet();

        foreach (var period in silentPeriods)
        {
            period.EndedOn = period.InstallLifecycle.LastSeen;
            period.EndReason = optedOutInstallIds.Contains(period.InstallLifecycle.InstallId)
                ? KavitaPlusPeriodEndReason.OptedOut
                : KavitaPlusPeriodEndReason.WentSilent;
        }

        await context.SaveChangesAsync();

        logger.LogInformation("Closed {Count} Kavita+ periods for installs silent for {Days} days",
            silentPeriods.Count, SilentPeriodCutoff.TotalDays);

        return silentPeriods.Count;
    }

    private static ReportedKavitaPlus? ToReportedKavitaPlus(KavitaPlusState? state, bool activeFlag)
    {
        return state switch
        {
            null => activeFlag
                ? new ReportedKavitaPlus(true, false, null)
                : new ReportedKavitaPlus(false, false, KavitaPlusPeriodEndReason.Unknown),
            KavitaPlusState.Active => new ReportedKavitaPlus(true, false, null),
            KavitaPlusState.Cancelling => new ReportedKavitaPlus(true, true, null),
            KavitaPlusState.Expired => new ReportedKavitaPlus(false, false, KavitaPlusPeriodEndReason.Expired),
            KavitaPlusState.Paused => new ReportedKavitaPlus(false, false, KavitaPlusPeriodEndReason.Paused),
            KavitaPlusState.None => new ReportedKavitaPlus(false, false, KavitaPlusPeriodEndReason.LicenseRemoved),
            KavitaPlusState.Unreachable => null,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }

    private static void ApplyKavitaPlusReport(InstallLifecycle lifecycle, ReportedKavitaPlus reported, bool? isFree,
        DateTime now)
    {
        var openPeriod = lifecycle.KavitaPlusPeriods.FirstOrDefault(p => p.EndedOn == null);

        if (!reported.HasAccess)
        {
            if (openPeriod == null) return;

            openPeriod.EndedOn = now;
            openPeriod.EndReason = reported.EndReason;
            return;
        }

        if (openPeriod == null)
        {
            var latestPeriod = lifecycle.KavitaPlusPeriods.MaxBy(p => p.StartedOn);
            if (latestPeriod?.EndReason == KavitaPlusPeriodEndReason.WentSilent)
            {
                latestPeriod.EndedOn = null;
                latestPeriod.EndReason = null;
                openPeriod = latestPeriod;
            }
            else
            {
                openPeriod = OpenPeriod(lifecycle, isFree, now);
            }
        }
        else if (isFree != null && openPeriod.IsFree != null && openPeriod.IsFree != isFree)
        {
            openPeriod.EndedOn = now;
            openPeriod.EndReason = KavitaPlusPeriodEndReason.PlanChanged;
            openPeriod = OpenPeriod(lifecycle, isFree, now);
        }

        openPeriod.IsFree ??= isFree;
        openPeriod.CancellingOn = reported.IsCancelling ? openPeriod.CancellingOn ?? now : null;
    }

    private static KavitaPlusPeriod OpenPeriod(InstallLifecycle lifecycle, bool? isFree, DateTime now)
    {
        var period = new KavitaPlusPeriod
        {
            StartedOn = now,
            IsFree = isFree
        };
        lifecycle.KavitaPlusPeriods.Add(period);
        return period;
    }
}
