using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MudBlazor;
using MudBlazor.Services;
using MyWatcher.Services;
using MyWatcherFrontend;
using MyWatcherFrontend.Services;

namespace MyWatcherFrontend
{
    public class Program
    {
        public static async Task Main(string[] args) 
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
                config.SnackbarConfiguration.VisibleStateDuration = 3000;
                config.SnackbarConfiguration.HideTransitionDuration = 400;
                config.SnackbarConfiguration.ShowTransitionDuration = 200;
            });
            builder.Services.AddSingleton<ICommunicationService, CommunicationService>();
            builder.Services.AddSingleton<IApiService, ApiService>();

            await builder.Build().RunAsync();
        }
    }
}
