using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyWatcherScraper.Services;

namespace MyWatcherScraper
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IScrapingService, ScrapingService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            //app
            Console.WriteLine("Heeeyy");
        }
    }
}