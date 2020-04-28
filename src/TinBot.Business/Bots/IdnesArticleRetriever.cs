using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TinBot.Business.Bots
{
    public class IdnesArticleRetriever
    {
        private readonly ILogger<IdnesArticleRetriever> _logger;
        private const string IdnesUrl = "https://www.idnes.cz/";

        public IdnesArticleRetriever(ILogger<IdnesArticleRetriever> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetMainArticleName()
        {
            var mainPageWeb = new HtmlWeb();
            var mainPageDocument = await mainPageWeb.LoadFromWebAsync(IdnesUrl);

            var mainTitle = mainPageDocument.DocumentNode.SelectSingleNode("//*[@id=\"votwyrak\"]/a/h3").InnerText;

            _logger.LogInformation("Main iDnes title is " + mainTitle);

            return mainTitle;
        }
    }
}
