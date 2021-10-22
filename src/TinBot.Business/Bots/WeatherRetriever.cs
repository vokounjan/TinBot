using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TinBot.DataAccess;

namespace TinBot.Business.Bots
{
    public class WeatherRetriever
    {
        private readonly ILogger<WeatherRetriever> _logger;
        private const string CzechWeatherUrl = "https://weather.com/cs-CZ/pocasi/dnes/l/EZXX0012:1:EZ";
        private const string EnglishWeatherUrl = "https://weather.com/en-US/pocasi/dnes/l/EZXX0012:1:EZ";

        //private const string PhraseXPath = "//div[@class = 'today_nowcard-phrase']";
        //private const string TemperatureXPath = "//div[@class = 'today_nowcard-temp']";

        private const string PhraseXPath = @"//div[@data-testid='wxPhrase']";
        private const string TemperatureXPath = @"//span[@data-testid='TemperatureValue']";

        public WeatherRetriever(ILogger<WeatherRetriever> logger)
        {
            _logger = logger;
        }

        public async Task<Weather> GetWeather(WeatherLanguage language)
        {
            var web = new HtmlWeb();
            var document = await web.LoadFromWebAsync(SelectUrl(language));

            var temperature = document.DocumentNode.SelectSingleNode(TemperatureXPath).InnerText;
            var phrase = document.DocumentNode.SelectSingleNode(PhraseXPath).InnerText;

            var weather = new Weather
            {
                Phrase = phrase,
                Temperature = temperature,
            };

            _logger.LogInformation(weather.ToString());

            return weather;
        }

        private string SelectUrl(WeatherLanguage language) => language switch
        {
            WeatherLanguage.Czech => CzechWeatherUrl,
            WeatherLanguage.English => EnglishWeatherUrl,
            _ => throw new InvalidEnumArgumentException(nameof(language), (int)language, typeof(WeatherLanguage)),
        };
    }

    public enum WeatherLanguage
    {
        Czech,
        English
    }

    public struct Weather
    {
        public string Phrase { get; set; }
        public string Temperature { get; set; }

        public override string ToString() => $"{Phrase} ({Temperature})";
    }
}