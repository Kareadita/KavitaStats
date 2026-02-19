using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OutputCaching;

namespace KavitaStats.Services;

public class InformativeOutputCachePolicy(TimeSpan timeSpan): IOutputCachePolicy
{
    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        context.AllowCacheLookup = true;
        context.AllowCacheStorage = true;
        context.ResponseExpirationTimeSpan = timeSpan;

        return ValueTask.CompletedTask;
    }

    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        context.HttpContext.Response.Headers["X-Cache-Status"] = "HIT";
        return ValueTask.CompletedTask;
    }

    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        context.HttpContext.Response.Headers["X-Cache-Status"] = "MISS";
        return ValueTask.CompletedTask;
    }
}
