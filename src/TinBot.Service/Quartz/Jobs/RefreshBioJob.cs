using Microsoft.Extensions.Logging;
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
                await A();
                var stopWatch = new Stopwatch();
                GC.Collect();
                stopWatch.Start();

                var nameDaysToday = await _nameDayRetriver.GetNameDay();
                var idnesArticle = await _idnesArticleRetriever.GetMainArticleName();
                var bitcoin = await _bitcoinPriceRetriever.GetBitcoinPrices();
                var moonPhase = await _moonPhaseRetriever.GetMoonPhase();
                var weatherCZ = await _weatherRetriever.GetWeather(WeatherLanguage.Czech);
                var weatherEN = await _weatherRetriever.GetWeather(WeatherLanguage.English);

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Jolandina poslední předpověd před smrtí");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Svého vysněného muže potkáš ve chvíli kdy:");
                stringBuilder.AppendLine($"• hodiny ukazují {DateTime.Now.AddMinutes(1).ToShortTimeString()}");
                stringBuilder.AppendLine($"• venku je {weatherCZ.ToString().LowercaseFirstChar()}");
                stringBuilder.AppendLine($"• svátek slaví {nameDaysToday[DateTime.Now.Millisecond % nameDaysToday.Count]}");
                stringBuilder.AppendLine($"• Bitcoin stojí {bitcoin.PriceUsd}");
                stringBuilder.AppendLine($"• {moonPhase}");
                //stringBuilder.AppendLine($"• iDnes říká '{idnesArticle}'");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Jo a prý se bude jmenovat Jan nebo tak nějak, tak to radši nepropásni.");

                var final = stringBuilder.ToString();
                stopWatch.Stop();

                Console.WriteLine(final);
                Console.WriteLine(final.Length);

                Console.WriteLine(stopWatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {

            }

            _logger.LogInformation("Bio refreshed!");
        }

        async Task A()
        {
            var request = new HttpRequestMessage
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
                    { "app-session-time-elapsed", "52844" },
                    { "user-session-time-elapsed", "52686" },
                    { "sec-fetch-dest", "empty" },
                    { "x-supported-image-formats", "webp,jpeg" },
                    { "tinder-version", "2.34.0" },
                    { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36" },
                    { "accept", "application/json" },
                    { "platform", "web" },
                    { "app-version", "1023400" },
                    { "origin", "https://tinder.com" },
                    { "sec-fetch-site", "cross-site" },
                    { "sec-fetch-mode", "cors" },
                    { "referer", "https://tinder.com/" },
                    { "accept-encoding", "gzip, deflate, br" },
                    { "accept-language", "cs-CZ,cs;q=0.9,en;q=0.8,es;q=0.7,sk;q=0.6" },
                },
                Content = new StringContent(@"{""user"":{""bio"":"".""}}", Encoding.UTF8, "application/json"),
            };


            var client = new HttpClient();
            var res1 = await client.SendAsync(request);

            request.Dispose();
        }
    }
}
