using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Common.Middlewares
{
    internal class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;


        public CustomExceptionHandlerMiddleware(RequestDelegate next, IHostEnvironment env,
            ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogDebug("Caught exception: '{Message}'. ", ex.Message);

            if (!context.Response.HasStarted)
            {
                _logger.LogError(ex, "An unexpected error occurred");

                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = StatusCodes.Status500InternalServerError;

                if (!_env.IsDevelopment())
                {
                    var externalResult = new
                    {
                        message = "An unexpected error has occurred",
                        success = false,
                    };

                    await response.WriteAsJsonAsync(externalResult).ConfigureAwait(false);
                    return;
                }

                var innerException = GetTheInnermostException(ex);

                var result = new
                {
                    details = innerException.StackTrace,
                    message = innerException.Message,
                    success = false
                };

                await response.WriteAsJsonAsync(result).ConfigureAwait(false);
            }
            else
            {
                _logger.LogWarning("Response has started. Cannot modify response header");
            }
        }

        private static Exception GetTheInnermostException(Exception exception)
        {
            var innerException = exception;

            while (innerException.InnerException is not null) innerException = innerException.InnerException;

            return innerException;
        }
    }
}