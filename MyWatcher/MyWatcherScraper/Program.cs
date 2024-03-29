﻿using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyWatcher.Services;
using MyWatcherScraper.Services;
using Serilog;
using Serilog.Events;
using Topshelf;

namespace MyWatcherScraper
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var services = RegisterServices();
            AddLogging();

            var rc = HostFactory.Run(x =>
            {
                x.Service<StartProgram>(sc =>
                {
                    var signalRSocket = services.GetRequiredService<ISignalRSocket>();

                    sc.ConstructUsing(name => new StartProgram(signalRSocket));
                    sc.WhenStarted((service, control) => service.Start(control));
                    sc.WhenStopped((service, control) => service.Stop(control));
                });

                x.SetDescription("Mywatcher - scraping tool");
                x.SetDisplayName("MyWatcher");
                x.SetServiceName("MyWatcher service");
                
                x.RunAsLocalService();
                x.StartAutomatically();
            });
            return 0;
        }

        private static IServiceProvider RegisterServices()
        {
            var serviceCollection = new ServiceCollection();
            IConfiguration configuration = SetupConfiguration();

            //serviceCollection.AddScoped(sp => new HttpClient { BaseAddress = new Uri(serviceCollection.HostEnvironment.BaseAddress) });
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<ISignalRSocket, SignalRSocket>();
            serviceCollection.AddTransient<IScrapingService, ScrapingService>();
            serviceCollection.AddTransient<IExtractService, ExtractService>();
            serviceCollection.AddTransient<ICommunicationService, CommunicationService>();
            serviceCollection.AddTransient<IApiService, ApiService>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        private static IConfiguration SetupConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json")
                .AddEnvironmentVariables()
                .Build();
            return builder;
        }

        private static void AddLogging()
        {
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
            }
        }
    }
}