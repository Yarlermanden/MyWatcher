using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
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
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });
            builder.Services.AddSingleton<ICommunicationService, CommunicationService>();
            builder.Services.AddSingleton<IApiService, ApiService>();
            //builder.Services.AddApiAuthorization("webApi", client => client.BaseAddress = new Uri("localhost:5000"));

            /*
            builder.Services.AddHttpClient("WebAPI", 
                    client => client.BaseAddress = new Uri("https://www.example.com/base"))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("WebAPI"));
            */

            await builder.Build().RunAsync();
        }
    }
}
