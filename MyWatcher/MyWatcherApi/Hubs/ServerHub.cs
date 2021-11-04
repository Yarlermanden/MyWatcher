using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
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
        Log.Information($"Client {Context.ConnectionId} disconnected");
        await base.OnConnectedAsync();
    }
    
    //=============================== REQUESTS TO SERVER =======================================================
    //Invoked from API calls made by the clients 
    public async Task StartUserScraping(string user, string message)
    {
        await Clients.All.SendAsync("StartUserScraping", user, message);
    }
}