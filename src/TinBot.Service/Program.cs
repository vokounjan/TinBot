using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace TinBot.Service
{
    public static class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                    services.AddServiceServices(hostContext.Configuration);
                    services.AddHostedService<Worker>();
                });
    }
}
