
using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyWatcher.Services;
using Serilog;
using Serilog.Events;

namespace MyWatcherApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
            var isDevelopment = environment == Environments.Development;
            
            if (isDevelopment)
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                    .Enrich.FromLogContext().WriteTo.Console()
                    //.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information)
                    .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                    .Enrich.FromLogContext().CreateLogger();
                /*
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(
                        new Uri(""))
                    {
                        ModifyConnectionSettings =
                            x => x.BasicAuthentication("elastic", ""),
                        AutoRegisterTemplate = true,
                        CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                        IndexFormat = $""
                    })
                .CreateLogger();
                */
            }
            
            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                try
                {
                    context.Database.Migrate();
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Failed to ensure the database was created");
                    Log.Error("{0} Failed to ensure the database was created", exception);
                    return 1;
                }
            }
            
            try
            {
                Log.Information("Starting up");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            return 0;
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).UseSerilog()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environment = hostingContext.HostingEnvironment.EnvironmentName;
                    config.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}