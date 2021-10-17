using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using MyWatcher.Models;
using MyWatcher.Services;
using System.Text.Json;

namespace MyWatcherScraper.Services
{
    public interface IApiService
    {
        public Task<List<ItemGetDTO>> GetAllItems(int serviceId);
        public Task UpdateItem(ItemUpdateDTO dto);
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

        public async Task<List<ItemGetDTO>> GetAllItems(int serviceId)
        {
            var response = await _communicationService.SendGetRequest($"/api/item/getAll/{serviceId}", "");
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
    }
}