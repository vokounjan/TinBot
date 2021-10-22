using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TinBot.Business.Bots;

namespace TinBot.Service.Jobs
{
    [DisallowConcurrentExecution]
    public class RefreshBioJob : IJob
    {
        private readonly ILogger<RefreshBioJob> _logger;
        private readonly NameDayRetriever _nameDayRetriver;
        private readonly IdnesArticleRetriever _idnesArticleRetriever;
        private readonly BitcoinPriceRetriever _bitcoinPriceRetriever;
        private readonly WeatherRetriever _weatherRetriever;
        private readonly MoonPhaseRetriever _moonPhaseRetriever;

        public RefreshBioJob(
            ILogger<RefreshBioJob> logger,
            NameDayRetriever nameDayRetriver,
            IdnesArticleRetriever idnesArticleRetriever,
            BitcoinPriceRetriever bitcoinPriceRetriever,
            WeatherRetriever weatherRetriever,
            MoonPhaseRetriever moonPhaseRetriever)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _nameDayRetriver = nameDayRetriver ?? throw new ArgumentNullException(nameof(nameDayRetriver));
            _idnesArticleRetriever = idnesArticleRetriever ?? throw new ArgumentNullException(nameof(idnesArticleRetriever));
            _bitcoinPriceRetriever = bitcoinPriceRetriever ?? throw new ArgumentNullException(nameof(bitcoinPriceRetriever));
            _weatherRetriever = weatherRetriever ?? throw new ArgumentNullException(nameof(weatherRetriever));
            _moonPhaseRetriever = moonPhaseRetriever ?? throw new ArgumentNullException(nameof(moonPhaseRetriever));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Refreshing bio...");

            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var time = DateTime.Now.AddMinutes(1).ToShortTimeString();
                var nameDaysToday = await _nameDayRetriver.GetNameDay();
                var bitcoin = await _bitcoinPriceRetriever.GetBitcoinPrices();
                var moonPhase = await _moonPhaseRetriever.GetMoonPhase();
                var weatherCZ = await _weatherRetriever.GetWeather(WeatherLanguage.Czech);

                //var idnesArticle = await _idnesArticleRetriever.GetMainArticleName();
                //var weatherEN = await _weatherRetriever.GetWeather(WeatherLanguage.English);

                var stringBuilder = new StringBuilder();
                stringBuilder.Append($"Ahoj, ");
                stringBuilder.Append($"hodiny ukazují {time}, ");
                stringBuilder.Append($"venku je {weatherCZ.ToString().LowercaseFirstChar()}, ");
                stringBuilder.Append($"Bitcoin stojí {bitcoin.PriceUsd} dolarů, ");
                stringBuilder.Append($"svátek slaví {nameDaysToday.First()} ");
                stringBuilder.Append("a já mám asi moc volnýho času. 🙃");
                stringBuilder.Append("\n\n");
                stringBuilder.Append("Takže - půjdeme na kafe, nebo na víno?");
                stringBuilder.Append("\n\n");
                stringBuilder.Append("🇨🇿🇬🇧🇪🇦");
                stringBuilder.Append("\n\n");
                stringBuilder.Append("184 cm");

                var text = stringBuilder.ToString();

                await UpdateBio(text);

                stopWatch.Stop();
                _logger.LogInformation($"Bio updated in {stopWatch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Bio update failed: {ex}");
                await UpdateBio(string.Empty);
            }
        }

        private async Task UpdateBio(string text)
        {
            var bio = new JObject(new JProperty("user", new JObject(new JProperty("bio", text))));
            var bioJson = JsonConvert.SerializeObject(bio, Formatting.None);

            using var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://api.gotinder.com/v2/profile?locale=cs"),
                Method = HttpMethod.Post,
                Headers =
                {
                    { "method", "POST" },
                    { "authority", "api.gotinder.com" },
                    { "scheme", "https" },
                    { "path", "/v2/profile?locale=cs" },
                    { "pragma", "no-cache" },
                    { "cache-control", "no-cache" },
                    { "dnt", "1" },
                    { "app-session-time-elapsed", "246964" },
                    { "x-auth-token", "841ae712-dddd-4a21-9a41-75e92366bb8f" },
                    { "user-session-time-elapsed", "246779" },
                    { "sec-fetch-dest", "empty" },
                    { "x-supported-image-formats", "jpeg" },
                    { "persistent-device-id", "2fe1a36c-8133-45fa-a336-77dbb143016b" },
                    { "tinder-version", "3.2.1" },
                    { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36" },
                    { "user-session-id", "63af1bfd-e1a0-4377-93c6-90b1110181bf" },
                    { "accept", "application/json" },
                    { "platform", "web" },
                    { "app-session-id", "78238a9a-61c7-4854-a5b9-84e66f7f40ac" },
                    { "app-version", "1030201" },
                    { "origin", "https://tinder.com" },
                    { "sec-fetch-site", "cross-site" },
                    { "sec-fetch-mode", "cors" },
                    { "referer", "https://tinder.com/" },
                    { "accept-encoding", "gzip, deflate, br" },
                    { "accept-language", "cs-CZ,cs;q=0.9,en;q=0.8,es;q=0.7,sk;q=0.6" },
                },
                Content = new StringContent(bioJson, Encoding.UTF8, "application/json"),
            };

            using var client = new HttpClient();
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException(response.ReasonPhrase);
        }
    }
}