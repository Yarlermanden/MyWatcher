using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using MyWatcher.Models;
using Newtonsoft.Json;

namespace MyWatcherScraper.Services
{
    public interface IRequestListenerService
    {
        public Task StartListening();
        public Task ForceRescan(ForceRescanRequest request);

    }

    public class RequestListenerService : IRequestListenerService
    {
        private TcpClient _client;
        private IScrapingService _scrapingService;
        
        public RequestListenerService(IScrapingService scrapingService)
        {
            _scrapingService = scrapingService;
        }

        public async Task StartListening()
        {
            var thread = new Thread(() => {
                try
                {
                    _client = new TcpClient();
                    //Todo use appsettings.json to determine where to connect
                    Console.WriteLine("Connected to api's websocket");

                    while (true)
                    {
                        if(!_client.Client.Connected) Connect();
                        var stream = _client.GetStream();
                        if (!stream.DataAvailable) continue;
                        
                        var data = new Byte[256];
                        var bytes = stream.Read(data, 0, data.Length);
                        var responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                        var request = JsonConvert.DeserializeObject<ForceRescanRequest>(responseData);

                        var response = "";
                        if (request != null)
                        {
                            response = "ok";
                        }
                        else response = "failed";

                        data = System.Text.Encoding.ASCII.GetBytes(response);
                        stream = _client.GetStream();
                        stream.Write(data, 0, data.Length);
                        
                        ForceRescan(request);
                        
                        /*
                        var bytes = stream.Read(data, 0, data.Length);
                        if (bytes == 0) continue;

                        BinaryFormatter bf = new BinaryFormatter();
                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            object obj = bf.Deserialize(ms);
                            ForceRescan((ForceRescanRequest)obj);
                        }
                        */
                        /*
                        var serializer = new DataContractJsonSerializer(typeof(ForceRescanRequest));
                        var request = serializer.ReadObject(stream) as ForceRescanRequest;
                        ForceRescan(request);
                        */
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed handling client request: {e.Message}");
                }
                finally
                {
                    _client.GetStream().Close();
                    _client.Client.Close();
                    _client.Close();
                }
            });
            thread.Start();
        }

        private void Connect()
        {
            _client.Client.Connect("127.0.0.1", 5005);
        }

        public async Task ForceRescan(ForceRescanRequest request)
        {
            await _scrapingService.ScrapeAllItemsOfService(request.ServiceId);
        }
    }
}