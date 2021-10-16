using System;
using Polly;
using Serilog;
using Topshelf;

namespace MyWatcherScraper.Services
{
    public class StartScrapingService : ServiceControl
    {
        private IScrapingService _scrapingService;

        public StartScrapingService(IScrapingService scrapingService)
        {
            _scrapingService = scrapingService;
        }

        public bool Start(HostControl hostControl)
        {
            var policy = Policy.Handle<Exception>().WaitAndRetryForeverAsync(
                attempt => TimeSpan.FromMilliseconds(2000), // Wait 2000ms between each try.
                (exception, calculatedWaitDuration) => // Capture some info for logging!
                {
                    Log.Error("Log, then retry: " + exception.Message, ConsoleColor.Yellow);
                });

            policy.ExecuteAsync(async () =>
            {
                //Todo Create websocket connection
                
                //Listen
                
                //_scrapingService
                Console.WriteLine("heylloolo");
                await _scrapingService.ScrapePrice(
                    "https://www.pricerunner.dk/pl/1-5261396/Mobiltelefoner/Apple-iPhone-12-64GB-Sammenlign-Priser");
                //await _scrapingService.Test();

            });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}