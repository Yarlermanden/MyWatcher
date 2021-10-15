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
using Microsoft.Bot.Connector.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyWatcher.Entities;
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
            services.AddControllers();
            services.AddAntiforgery(options => { options.Cookie.SecurePolicy = CookieSecurePolicy.Always; });
            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
            Configuration.Bind("Authentication", authenticationConfiguration);
            services.AddSingleton(authenticationConfiguration);
           
            services.AddDbContextFactory<DatabaseContext>(options =>
            {
                //options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("MyWatcherApi"));
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
                //Run migrations with dotnet ef --startup-project ../MyWatcherApi migrations add Initial
            });
            services.AddScoped(p =>
                p.GetRequiredService<IDbContextFactory<DatabaseContext>>()
                    .CreateDbContext());

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

            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IUserItemService, UserItemService>();
            services.AddTransient<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app, IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            app.UseRouting();
            //app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                /*
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=api}/{action=Index}/{id?}"
                );
                */
                endpoints.MapControllers();
            });
            ApplicationDbInitializer.SeedUsers(dbContextFactory);
        }

        public static class ApplicationDbInitializer
        {
            public static void SeedUsers(IDbContextFactory<DatabaseContext> factory)
            {
                using var dbContext = factory.CreateDbContext();
                var users = dbContext.Users.Select(u => u).ToList();
                if (users == null || users.Count == 0)
                {
                    var user = new User()
                    {
                        Name = "Martin Holst",
                        UserName = "Yarl",
                        Password = "Hej123!"
                    };

                    var service = new Service()
                    {
                        Name = "PriceRunner"
                    };

                    dbContext.Users.Add(user);
                    dbContext.Services.Add(service);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}