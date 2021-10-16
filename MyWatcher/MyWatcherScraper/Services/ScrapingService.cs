using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyWatcherScraper.Services
{
    public interface IScrapingService
    {
        public Task Test();
        //Find urls for items
        
        //Find price from urls
        public Task<double> ScrapePrice(string url);
    }
    
    public class ScrapingService : IScrapingService
    {
        private HttpClient _client;
        private IExtractService _extractService;
        public ScrapingService(IExtractService extractService)
        {
            _client = new HttpClient();
            _extractService = extractService;
        }

        public async Task<double> ScrapePrice(string url)
        {
            var response = await _client.GetStringAsync(url);
            var html = getAsHtmlDocument(response);
            return await _extractService.ExtractPriceFromPricerunner(html);
        }

        public async Task Test()
        {
            //string fullUrl = "https://pricerunner.com/robots.txt";
            string fullUrl =
                "https://www.pricerunner.dk/pl/1-5261396/Mobiltelefoner/Apple-iPhone-12-64GB-Sammenlign-Priser";
            var response = await _client.GetStringAsync(fullUrl);

            getAsHtmlDocument(response);
        }

        private HtmlAgilityPack.HtmlDocument getAsHtmlDocument(string htmlData)
        {
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(htmlData);
            return htmlDocument;
        }
    }
}