using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyWatcher.Models;
using System.Text.Json;
using MyWatcher.Entities;

namespace MyWatcherFrontend.Services
{
    public interface IApiService
    {
        public Task<List<UserItemTableDTO>> GetUserItems(User user);
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
        
        public async Task<List<UserItemTableDTO>> GetUserItems(User user)
        {
            var userId = user.Id;
            var response = await _communicationService.SendGetRequest($"/api/useritem/getpricerunner/{userId}", "");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonString);
                var items = JsonSerializer.Deserialize<List<UserItemTableDTO>>(jsonString, _options);
                return items;
            }
            return null;
        }
    }
}