using Microsoft.Extensions.Configuration;
using TinBot.Business.Bots;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataContextServices(configuration);

            services.AddScoped<NameDayRetriever>();
            services.AddTransient<IdnesArticleRetriever>();
            services.AddTransient<BitcoinPriceRetriever>();
            services.AddTransient<MoonPhaseRetriever>();
            services.AddTransient<WeatherRetriever>();

            return services;
        }
    }
}
