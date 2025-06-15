using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Extensions;
using Talabat.Middlewares;
using Talabat.Repository.Context;

namespace Talabat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.ApplyServices();
            //DI TalabatContext
            builder.Services.AddDbContext<TalabatContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //DI Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connect = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(connect);
            });
            // DI TalabatContextIdentity
            builder.Services.AddDbContext<TalabatContextIdentity>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionIdentity"));
            });
            //authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    };
                });
            //emailsetting
            builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("Email"));

              
            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.ApplySwaggerServices();

            var app = builder.Build();
            var scop = app.Services.CreateScope();
            var services = scop.ServiceProvider; //DI
            var logger = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var talabatDBcontext = services.GetRequiredService<TalabatContext>();
                await talabatDBcontext.Database.MigrateAsync();

                var talabatDBcontextIdentity=services.GetRequiredService<TalabatContextIdentity>();   
                await talabatDBcontextIdentity.Database.MigrateAsync();

                await TalabatContextDataSeed.SeedData(talabatDBcontext);

            }catch(Exception ex)
            {
                var log = logger.CreateLogger<Program>();
                log.LogError(ex, "Error During Migration!!");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.ApplySwaggerServices();
            }
            
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.MapControllers();

            app.Run();
        }
    }
}
