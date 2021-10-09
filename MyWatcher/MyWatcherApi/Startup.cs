using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyWatcher.Entities;
using MyWatcher.Services;

//using Microsoft.Extensions.Hosting;

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
                //options.UseNpgsql(
                //"Server=localhost;Database=mywatch;Port=5432;User Id=postgres;Password=Pengeko2;Include Error Detail=true; Ssl Mode=Allow");
                //options.UseNpgsql(
                //"Server=http://192.168.0.83;Port=5432;Database=mywatch;User Id=postgres;Password=Pengeko2;Include Error Detail=true;");
                //"postgres@192.168.0.83:5432"
                //"postgresql://192.168.0.83/mywatch?user=postgres&password=Pengeko2");
                //"postgres://{postgres}:{Pengeko2}@{192.168.0.83}:{5432}/{mywatch}");
                //"postgres://postgres:Pengeko2@192.168.0.83:5432/mywatch");
                
                //Run migrations with dotnet ef --startup-project ../MyWatcherApi migrations add Initial
            });
            services.AddScoped(p =>
                p.GetRequiredService<IDbContextFactory<DatabaseContext>>()
                    .CreateDbContext());

            /*
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
            */
            
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


            //services.AddTransient

        }

        public void Configure(IApplicationBuilder app, IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            ApplicationDbInitializer.SeedUsers(dbContextFactory);
        }

        public static class ApplicationDbInitializer
        {
            public static void SeedUsers(IDbContextFactory<DatabaseContext> factory)
            {
                using var dbContext = factory.CreateDbContext();
                var user = dbContext.Users.SingleOrDefault(u => u.UserName == "Yarl");
                if (user == null)
                {
                    user = new User()
                    {
                        Name = "Martin Holst",
                        UserName = "Yarl",
                        Password = "Pengeko2"
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