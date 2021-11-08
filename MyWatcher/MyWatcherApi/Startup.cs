using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
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
        }

        public void Configure(IApplicationBuilder app, DatabaseContext dbContext, IUserService userService)
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
            ApplicationDbInitializer.SeedUsers(dbContext, userService);
        }

        public static class ApplicationDbInitializer
        {
            public static void SeedUsers(DatabaseContext dbContext, IUserService userService)
            {
                var users = dbContext.Users.Select(u => u).ToList();
                if (users == null || users.Count == 0)
                {
                    var user = new UserRegisterDTO()
                    {
                        UserName = "Yarl",
                        Password = "Hej123!",
                        Email = "Test@gmail.com",
                    };

                    userService.RegisterUser(user);
                }
            }
        }
    }
}