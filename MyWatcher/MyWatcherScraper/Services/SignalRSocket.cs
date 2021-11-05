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
    public HubConnection HubConnection { get; set; }

    public SignalRSocket()
    {
        
    }

    public async Task Connect()
    {
        HubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:5001/ServerHub")
            //.AddMessagePackProtocol()
            .Build();

        HubConnection.On<string>("StartUserScraping", (s) =>
        {
            var request = JsonSerializer.Deserialize<ForceRescanRequest>(s);
            Console.WriteLine($"Received {request} via SignalR");
        });

        await HubConnection.StartAsync();
        Log.Information("SignalR is connected");
    }
}