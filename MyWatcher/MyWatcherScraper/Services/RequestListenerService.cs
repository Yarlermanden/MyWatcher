using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MyWatcher.Models;

namespace MyWatcherScraper.Services
{
    public interface IRequestListenerService
    {
        public Task ForceRescan(ForceRescanRequest request);

    }

    public class RequestListenerService : IRequestListenerService
    {
        private TcpClient _client;
        
        public RequestListenerService()
        {
            var thread = new Thread(() => {
                _client = new TcpClient();
                //Todo use appsettings.json to determine where to connect
                _client.Client.Connect("127.0.0.1", 5005);
                Console.WriteLine("Connected to api's websocket");
            });
            thread.Start();
        }

        public async Task ForceRescan(ForceRescanRequest request)
        {
            
        }
    }
}