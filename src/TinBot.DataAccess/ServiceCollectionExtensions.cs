using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TinBot.DataAccess;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TinContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(TinContext))));

            return services;
        }
    }
}
