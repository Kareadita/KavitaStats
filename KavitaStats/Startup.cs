using System;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Hangfire;
using KavitaStats.Extensions;
using KavitaStats.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Serilog;

namespace KavitaStats;

public class Startup
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration config, IWebHostEnvironment env)
    {
        _config = config;
        _env = env;
        
        // Disable Hangfire Automatic Retry
        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
    }
        
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplicationServices(_config, _env);
        services.AddControllers();
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.All;
        });
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(
                        "https://github.com",
                        "https://www.github.com",
                        "https://kavitastats.com",
                        "https://www.kavitastats.com"
                    )
                    .WithExposedHeaders("Content-Disposition", "Pagination", "x-api-key", "api-key");
            });
        });
        services.AddIdentityServices(_config);
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v2", new OpenApiInfo {Title = "KavitaStats", Version = "v2"});
            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert the API KEY into field",
                Name = "x-api-key",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement((document) => new OpenApiSecurityRequirement()
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });
        });
            
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes =
                ResponseCompressionDefaults.MimeTypes.Concat(
                    ["image/jpeg", "image/jpg"]);
            options.EnableForHttps = true;
        });
        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        services.AddResponseCaching();
        
        services.AddHangfire(configuration => configuration
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseInMemoryStorage());
        services.AddHangfireServer();
        
        services.AddHostedService<StartupTasksHostedService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
        IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KavitaStats v3"));
            
            app.UseHangfireDashboard();
        }
            
        app.UseResponseCompression();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseRouting();
        
        app.UseCors();
            
        app.UseResponseCaching();

        app.UseAuthentication();

        app.UseAuthorization();
            
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = new FileExtensionContentTypeProvider()
        });
            
        app.UseSerilogRequestLogging();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
            
        applicationLifetime.ApplicationStopping.Register(OnShutdown);
        applicationLifetime.ApplicationStarted.Register(() =>
        {
            Console.WriteLine($"KavitaStats - v{Assembly.GetExecutingAssembly().GetName().Version}");
        });
    }
        
    private static void OnShutdown()
    {
        Console.WriteLine("Server is shutting down. Please allow a few seconds to stop any background jobs...");
        System.Threading.Thread.Sleep(1000);
        Console.WriteLine("You may now close the application window.");
    }
}