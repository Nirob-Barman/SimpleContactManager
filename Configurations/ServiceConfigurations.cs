using Microsoft.EntityFrameworkCore;
using SimpleContactManager.Data;

namespace SimpleContactManager.Configurations
{
    public static class ServiceConfigurations
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext with the connection string
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add other services here as needed

            return services;
        }
    }
}
