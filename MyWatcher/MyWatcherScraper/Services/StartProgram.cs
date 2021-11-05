using System;
using Polly;
using Serilog;
using Topshelf;

namespace MyWatcherScraper.Services
{
    public class StartProgram : ServiceControl
    {
        private readonly ISignalRSocket _signalRSocket;

        public StartProgram(ISignalRSocket signalRSocket)
        {
            _signalRSocket = signalRSocket;
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
                var serviceId = 1; 

                await _signalRSocket.Connect();
            });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}