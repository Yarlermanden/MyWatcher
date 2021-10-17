using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MyWatcher.Models;

namespace MyWatcher.Services
{
    public interface IScraperSocketService
    {
        public Task<bool> StartScrapingOfUserItems(ForceRescanRequest request);
    }
    
    public class ScraperSocketService : IScraperSocketService
    {
        //TOdo define this address from the appsettings.json
        private TcpListener _server;
        private TcpClient _client;

        public ScraperSocketService()
        {
            _server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5005);
            //spin new thread
            var thread = new Thread(() =>
            {
                _server.Start();
                _client = _server.AcceptTcpClient();
            });
            thread.Start();
            //_server.Start();
        }

        public async Task<bool> StartScrapingOfUserItems(ForceRescanRequest request)
        {
            if (_client == null) return false;

            return true;
        }
        
    }
}