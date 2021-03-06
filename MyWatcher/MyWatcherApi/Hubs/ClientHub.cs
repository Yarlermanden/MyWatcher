using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MyWatcher.Models;
using MyWatcher.Models.Enums;
using Serilog;

namespace MyWatcherApi.Hubs
{
    public class ClientHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var group = Context.GetHttpContext().Request.Query["userId"];
            Log.Information($"Client {Context.ConnectionId} connected belonging to group {group}");
    
            string value = !string.IsNullOrEmpty(group.ToString()) ? group.ToString() : "default";
    
            await Groups.AddToGroupAsync(Context.ConnectionId, value);
            await base.OnConnectedAsync();
        }
            
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Log.Information($"Client {Context.ConnectionId} disconnected");
            await base.OnDisconnectedAsync(exception);
        }

        //=============================== REQUESTS TO CLIENT =======================================================
        public async Task ScrapingFinished(ScrapingCompleteDTO dto)
        {
            var endpoint = dto.Service == Service.Stock ? "StockScrapingFinished" : "SecondHandScrapingFinished";
            if (dto.UserId == null) await Clients.All.SendAsync(endpoint);
            else await Clients.Group(dto.UserId.ToString()).SendAsync(endpoint);
        }
    }
}