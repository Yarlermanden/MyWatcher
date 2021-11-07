using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyWatcher.Models;
using MyWatcher.Models.Enums;
using MyWatcher.Models.Item;

namespace MyWatcherScraper.Services
{
    public interface IScrapingService
    {
        //Find urls for items
        
        //Find price from urls
        public Task ScrapeAllItemsOfService(Service service);
        public Task ForceScrapeAllItemsOfService(ForceRescanRequest request);
        public Task<double> ScrapePrice(string url);
    }
    
    public class ScrapingService : IScrapingService
    {
        private HttpClient _client;
        private IExtractService _extractService;
        private IApiService _apiService;
        public ScrapingService(IExtractService extractService, IApiService apiService)
        {
            _client = new HttpClient();
            _extractService = extractService;
            _apiService = apiService;
        }

        public async Task ScrapeAllItemsOfService(Service service)
        {
            var items = await _apiService.GetAllItems(service);
            await ScrapeAllItems(items);
            await _apiService.SendScrapingComplete(null, service);
        }

        public async Task ForceScrapeAllItemsOfService(ForceRescanRequest request)
        {
            var items = await _apiService.GetAllItemsFromUserAndServiceNotRecentlyScanned(request);
            await ScrapeAllItems(items);
            await _apiService.SendScrapingComplete(request.UserId, request.Service);
        }

        public async Task ScrapeAllItems(List<ItemGetDTO> items)
        {
            foreach (var item in items)
            {
                var price = await ScrapePrice(item.URL);
                if (price != -1)
                {
                    var dto = new ItemUpdateDTO()
                    {
                        Id = item.Id,
                        Price = price
                    };
                    await _apiService.UpdateItem(dto);
                }
            }
        }
        
        public async Task<double> ScrapePrice(string url)
        {
            try
            {
                var response = await _client.GetStringAsync(url);
                var html = getAsHtmlDocument(response);
                return await _extractService.ExtractPriceFromPricerunner(html);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed scraping price due to {e.Message}");
                return -1;
            }
        }

        private HtmlAgilityPack.HtmlDocument getAsHtmlDocument(string htmlData)
        {
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(htmlData);
            return htmlDocument;
        }
    }
}