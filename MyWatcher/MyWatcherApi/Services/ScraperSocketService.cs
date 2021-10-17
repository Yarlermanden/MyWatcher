using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyWatcher.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Serilog;

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
            _server.ExclusiveAddressUse = true;
            Log.Information($"Now listening on: {_server.LocalEndpoint}");
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
            var jsonData = JsonConvert.SerializeObject(request);
            //byte[] dataBytes = Encoding.Default.GetBytes(jsonData);
            var data = System.Text.Encoding.ASCII.GetBytes(jsonData);
            
            var stream = _client.GetStream();
            stream.Write(data, 0, data.Length);
            //await stream.FlushAsync();

            data = new Byte[256];
            String responseData = String.Empty;
            var bytes = await stream.ReadAsync(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine($"Received: {responseData}");
            if (responseData == "ok") return true;
            else return false;
        }
    }
}