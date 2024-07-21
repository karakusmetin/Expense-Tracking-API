using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ET.Data.Extensions
{
    public static class DataLayerExtensions
    {
        public static IServiceCollection LoadDataLayerExtension(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ETDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("MsSqlConnection")));
            return services;
        }
    }
}
