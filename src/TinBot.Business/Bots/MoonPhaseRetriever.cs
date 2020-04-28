using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TinBot.Business.Bots
{
    public class MoonPhaseRetriever
    {
        private readonly ILogger<MoonPhaseRetriever> _logger;
        private const string MoonPhaseUrl = "https://www.online-siesta.com/faze-mesice/";
        private const string MoonPhaseXPath = "/html/body/div[2]/div[1]/h2";

        public MoonPhaseRetriever(ILogger<MoonPhaseRetriever> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetMoonPhase()
        {
            var web = new HtmlWeb();
            var document = await web.LoadFromWebAsync(MoonPhaseUrl);

            var moonPhase = document.DocumentNode.SelectSingleNode(MoonPhaseXPath).InnerText;

            _logger.LogInformation("Moon phase is " + moonPhase);

            return moonPhase;
        }
    }
}
