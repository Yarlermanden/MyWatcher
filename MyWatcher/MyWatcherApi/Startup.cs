using System;
using System.Collections.Generic;
using System.Fabric.Query;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyWatcher.Entities;
using MyWatcher.Models.User;
using MyWatcherApi.Hubs;
using MyWatcher.Services;
using MyWatcherApi.Api;

namespace MyWatcherApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services.AddControllersWithViews();
            services.AddAntiforgery(options => { options.Cookie.SecurePolicy = CookieSecurePolicy.Always; });
            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
            Configuration.Bind("Authentication", authenticationConfiguration);
            services.AddSingleton(authenticationConfiguration);

            services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", opt => opt
                    .WithOrigins("https://localhost:5003", "http://localhost:5002")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddAuthentication().AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                    //ValidIssuer = authenticationConfiguration.Issuer,
                    //ValidAudience = authenticationConfiguration.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSignalR(e =>
            {
                e.MaximumReceiveMessageSize = 100_000;
            }).AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNameCaseInsensitive = true;
            });
                //.AddMessagePackProtocol();
            //services.AddSignalR();

            services.AddRazorPages();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddSingleton<ServerHub>();
            services.AddSingleton<ClientHub>();
            services.AddTransient<IStockItemService, StockItemService>();
            services.AddTransient<IUserStockItemService, UserStockItemService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWebsiteService, WebsiteService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IParsingCsvService, ParsingCsvService>();
        }

        public void Configure(IApplicationBuilder app, DatabaseContext dbContext, IUserService userService, 
            IParsingCsvService parsingCsvService)
        {
            //app.UseHttpsRedirection();
            //app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseResponseCompression();
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ClientHub>("/ClientHub");
                endpoints.MapHub<ServerHub>("/ServerHub");
                //endpoints.MapBlazorHub();
                /*
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=api}/{action=Index}/{id?}"
                );
                */
            });
            try
            {
                //Task.Run(() => ApplicationDbInitializer.ParseAllSetupFilesAndSeed(dbContext, parsingCsvService, userService)).RunSynchronously();
                ApplicationDbInitializer.ParseAllSetupFilesAndSeed(dbContext, parsingCsvService, userService);
            }
            catch (Exception e)
            {
                Console.WriteLine("failed with: " + e.Message);
            }
        }

        public static class ApplicationDbInitializer
        {
            public static void SeedUsers(DatabaseContext dbContext, IUserService userService)
            {
                Console.WriteLine("seed started");
                var users = dbContext.Users.Select(u => u).ToList();
                if (users == null || users.Count == 0)
                {
                    var user = new UserRegisterDTO()
                    {
                        UserName = "Yarl",
                        Password = "Hej123!",
                        Email = "Test@gmail.com",
                    };

                    //userService.RegisterUser(user);
                }
            }

            public static void ParseAllSetupFilesAndSeed(DatabaseContext dbContext, IParsingCsvService parsingCsvService, IUserService userService)
            {
                //If users == null
                
                parsingCsvService.ParsingContinentsFromCsv(dbContext);
                parsingCsvService.ParsingCountriesFromCsv(dbContext);
                parsingCsvService.ParsingWebsitesFromCsv(dbContext);
                SeedUsers(dbContext, userService);
            }
        }
    }
}