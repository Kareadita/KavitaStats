using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KavitaStats.Services;
#nullable enable

/// <summary>
/// Runs on startup to schedule any reoccurring background jobs in KavitaStats
/// </summary>
/// <param name="serviceProvider"></param>
public class StartupTasksHostedService(IServiceProvider serviceProvider, ILogger<StartupTasksHostedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var taskScheduler = scope.ServiceProvider.GetRequiredService<ITaskScheduler>();
        taskScheduler.ScheduleTasks();

        await WarmCacheAsync(cancellationToken);
    }

    private async Task WarmCacheAsync(CancellationToken cancellationToken)
    {
        try
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var cacheService = scope.ServiceProvider.GetRequiredService<IUiStatsCacheService>();

            logger.LogInformation("Pre-warming UI stats cache...");
            await cacheService.GetActiveInstallsAsync();
            await cacheService.GetTotalInstallsAsync();
            logger.LogInformation("UI stats cache pre-warm complete");
        }
        catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogError(ex, "Failed to pre-warm UI stats cache");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
