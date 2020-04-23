using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TwitterBot.DataAccess;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TwitterContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(TwitterContext))));

            return services;
        }
    }
}
