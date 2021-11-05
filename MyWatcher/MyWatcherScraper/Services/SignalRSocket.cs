using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using MyWatcher.Models;
using System.Text.Json;
using Serilog;

namespace MyWatcherScraper.Services;

public interface ISignalRSocket
{
    Task Connect();
}

public class SignalRSocket : ISignalRSocket
{
    //private ConnectionInfo _connection;
    //private readonly ConfigService _configService;
    private readonly IScrapingService _scrapingService;
    public HubConnection HubConnection { get; set; }

    public SignalRSocket(IScrapingService scrapingService)
    {
        _scrapingService = scrapingService;
    }

    public async Task Connect()
    {
        HubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5001/ServerHub")
            //.AddMessagePackProtocol()
            .Build();

        HubConnection.On<string>("StartUserScraping", async (s) =>
        {
            var request = JsonSerializer.Deserialize<ForceRescanRequest>(s);
            Console.WriteLine($"Received {request} via SignalR");
            if (request == null) return;
            await _scrapingService.ForceScrapeAllItemsOfService(request);
        });

        await HubConnection.StartAsync();
        Log.Information("SignalR is connected");
    }
}