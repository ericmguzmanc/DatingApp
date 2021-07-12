using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection    AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // ℹ️ AddScoped is perfect because it only lives while request lifetime
            services.AddScoped<ITokenService, TokenService>();
            // ℹ️ Order does not matter here
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}