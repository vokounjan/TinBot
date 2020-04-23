using Microsoft.Extensions.Configuration;
using TwitterBot.Business.Bots;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataContextServices(configuration);

            services.AddTransient<AdamVojtechNeBot>();

            return services;
        }
    }
}
