using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MyWatcher.Models;
using System.Text.Json;
using Serilog;

namespace MyWatcherApi.Hubs;

public class ServerHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        Log.Information($"Server {Context.ConnectionId} connected");

        await Groups.AddToGroupAsync(Context.ConnectionId, "server");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Log.Information($"Server {Context.ConnectionId} disconnected");
        await base.OnConnectedAsync();
    }
    
    //=============================== REQUESTS TO SERVER =======================================================
    //Invoked from API calls made by the clients 
    //public async Task StartUserScraping(string user, string message)
    public async Task StartUserScraping(ForceRescanRequest request)
    {
        var s = JsonSerializer.Serialize(request);
        await Clients.All.SendAsync("StartUserScraping", s);
    }
}