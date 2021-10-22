using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TinBot.Business.Bots
{
    public class BitcoinPriceRetriever
    {
        private readonly ILogger<IdnesArticleRetriever> _logger;
        private const string BitcoinUrl = "https://www.kurzy.cz/bitcoin/";
        private const string UsdXPath = "//*[@id=\"last_usd\"]";
        private const string CzkXPath = "//*[@id=\"last_czk\"]";

        public BitcoinPriceRetriever(ILogger<IdnesArticleRetriever> logger)
        {
            _logger = logger;
        }

        public async Task<Bitcoin> GetBitcoinPrices()
        {
            var web = new HtmlWeb();
            var document = await web.LoadFromWebAsync(BitcoinUrl);

            var czkPrice = document.DocumentNode.SelectSingleNode(CzkXPath).InnerText.Split('.')[0];
            var usdPrice = document.DocumentNode.SelectSingleNode(UsdXPath).InnerText.Split('.')[0];

            _logger.LogInformation($"Bitcoin price: ${usdPrice} / {czkPrice} CZK");

            return new Bitcoin
            {
                PriceCzk = czkPrice,
                PriceUsd = usdPrice,
            };
        }
    }

    public struct Bitcoin
    {
        public string PriceCzk { get; set; }
        public string PriceUsd { get; set; }
    }
}