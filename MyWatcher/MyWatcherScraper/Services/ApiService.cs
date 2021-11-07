using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using MyWatcher.Models;
using MyWatcher.Services;
using System.Text.Json;
using MyWatcher.Models.Enums;
using MyWatcher.Models.Item;

namespace MyWatcherScraper.Services
{
    public interface IApiService
    {
        public Task<List<ItemGetDTO>> GetAllItems(Service service);
        public Task<List<ItemGetDTO>> GetAllItemsFromUserAndServiceNotRecentlyScanned(ForceRescanRequest forceRescanRequest);
        public Task UpdateItem(ItemUpdateDTO dto);
        public Task SendScrapingComplete(Guid? userId, Service service);
    }
    
    public class ApiService : IApiService
    {
        private ICommunicationService _communicationService;
        private readonly JsonSerializerOptions _options;

        public ApiService(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
            _options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<ItemGetDTO>> GetAllItems(Service service)
        {
            var response = await _communicationService.SendGetRequest($"/api/item/getAll/{service}", "");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully retrieved all items");
                var jsonString = await response.Content.ReadAsStringAsync();
                var items = JsonSerializer.Deserialize<List<ItemGetDTO>>(jsonString, _options);
                return items;
            }
            Console.WriteLine("Failed retrieving all items");
            return null;
        }

        public async Task<List<ItemGetDTO>> GetAllItemsFromUserAndServiceNotRecentlyScanned(
            ForceRescanRequest forceRescanRequest)
        {
            var response = await _communicationService.SendGetRequest($"/api/item/getFromUserNotRecentlyUpdated/{forceRescanRequest.Service}/{forceRescanRequest.UserId}", "");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully retrieved all items");
                var jsonString = await response.Content.ReadAsStringAsync();
                var items = JsonSerializer.Deserialize<List<ItemGetDTO>>(jsonString, _options);
                return items;
            }
            Console.WriteLine("Failed retrieving all items");
            return null;
        }

        public async Task UpdateItem(ItemUpdateDTO dto)
        {
            var response = await _communicationService.PatchRequest($"/api/item/updateItem", dto);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully updated item");
            }
            else
            {
                Console.WriteLine("Failed to update item");
            }
        }

        public async Task SendScrapingComplete(Guid? userId, Service service)
        {
            ScrapingCompleteDTO dto = new ScrapingCompleteDTO() {UserId = userId, Service = service};
            var response = await _communicationService.PostItemRequest($"/api/item/scrapingCompleted", dto);
            if (response.IsSuccessStatusCode) Console.WriteLine("Successfully completed scraping");
            else Console.WriteLine("Failed completing scraping");
        }
    }
}