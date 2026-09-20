using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;

namespace KavitaStats.Middleware;

/// <summary>
/// Guards /mcp. Accepts the passcode as <c>Authorization: Bearer {passcode}</c> or as a trailing path segment, <c>/mcp/{passcode}</c>
/// </summary>
public class McpPasscodeMiddleware(RequestDelegate next, IConfiguration config)
{
    public const string McpPath = "/mcp";
    private const string BearerPrefix = "Bearer ";

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments(McpPath, out var remaining))
        {
            await next(context);
            return;
        }

        var expected = config.GetValue<string>("McpPasscode");
        var provided = ExtractPasscode(context, remaining);

        if (string.IsNullOrWhiteSpace(expected) || provided == null || !PasscodesMatch(expected, provided))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await next(context);
    }

    private static string ExtractPasscode(HttpContext context, PathString remaining)
    {
        if (remaining.HasValue && remaining.Value.Length > 1)
        {
            context.Request.Path = McpPath;

            // Serilog request logging reads RawTarget, which would otherwise write the passcode to the log
            var requestFeature = context.Features.Get<IHttpRequestFeature>();
            if (requestFeature != null) requestFeature.RawTarget = McpPath;

            return remaining.Value[1..];
        }

        var authHeader = context.Request.Headers.Authorization.ToString();
        return authHeader.StartsWith(BearerPrefix) ? authHeader[BearerPrefix.Length..].Trim() : null;
    }

    private static bool PasscodesMatch(string expected, string provided)
    {
        return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(expected), Encoding.UTF8.GetBytes(provided));
    }
}
