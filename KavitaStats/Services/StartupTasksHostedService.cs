using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KavitaStats.Services;
#nullable enable

/// <summary>
/// Runs on startup to schedule any reoccurring background jobs in KavitaStats
/// </summary>
/// <param name="serviceProvider"></param>
public class StartupTasksHostedService(IServiceProvider serviceProvider) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var taskScheduler = scope.ServiceProvider.GetRequiredService<ITaskScheduler>();
        taskScheduler.ScheduleTasks();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
