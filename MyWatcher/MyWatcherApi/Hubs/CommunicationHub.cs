using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace MyWatcherApi.Hubs
{
    public class CommunicationHub : Hub
    {
        /*
        public override async Task OnConnectedAsync()
        {
            Log.Information($"Client {Context.ConnectionId} connected");
            var group = Context.GetHttpContext().Request.Query["userId"];
    
            string value = !string.IsNullOrEmpty(group.ToString()) ? group.ToString() : "default";
    
            await Groups.AddToGroupAsync(Context.ConnectionId, value);
            await base.OnConnectedAsync();
        }
            
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Log.Information($"Client {Context.ConnectionId} disconnected");
            await base.OnDisconnectedAsync(exception);
        }
        */

        //=============================== REQUESTS TO SERVER =======================================================
        //Invoked from API calls made by the clients 
        public async Task StartUserScraping(string user, string message)
        {
            await Clients.All.SendAsync("StartUserScraping", user, message);
        }



        //=============================== REQUESTS TO CLIENT =======================================================
        //Invoked from API calls made by the server
        public async Task ScrapingFinished()
        {
            await Clients.All.SendAsync("ScrapingFinished");
        }
    }
}