using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MyWatcher.Models;
using System.Text.Json;
using MyWatcher.Entities;
using Npgsql.Replication;

namespace MyWatcherFrontend.Services
{
    public interface IApiService
    {
        public Task<List<UserItemTableDTO>> GetUserItems(User user);
        public Task<(bool, int, string)> AddUserItem(UserItemAddDTO dto);
        public Task<(bool, string)> DeleteUserItem(UserItemDeleteDTO dto);
        public Task<(bool, string)> UpdateUserItem(UserItemUpdateDTO dto);
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

        public async Task<(bool, int, string)> AddUserItem(UserItemAddDTO dto)
        {
            var response = await _communicationService.PostItemRequest($"/api/useritem/add", dto);
            if (response.IsSuccessStatusCode)
            {
                var id = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Successfully posted Item with id: {id}"); 
                
                return (response.IsSuccessStatusCode, int.Parse(id), "");
            }
            else
            {
                Console.WriteLine("Failed posting Item");
                var errorMessage = "";
                if (response.StatusCode == HttpStatusCode.Conflict) errorMessage = "Item already exists";
                return (response.IsSuccessStatusCode, -1, errorMessage);
            }
        }

        public async Task<(bool, string)> DeleteUserItem(UserItemDeleteDTO dto)
        {
            var response = await _communicationService.DeleteItemRequest($"/api/useritem/delete", dto);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully deleted item");
                return (true, "");
            }
            else
            {
                Console.WriteLine("Failed deleting Item");
                //return (false, "This item can't be deleted");
                return (false, "Failed to delete this item");
            }
        }

        public async Task<(bool, string)> UpdateUserItem(UserItemUpdateDTO dto)
        {
            var response = await _communicationService.UpdateItemRequest($"/api/useritem/update", dto);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully updated Item");
                return (true, "");
            }
            else
            {
                Console.WriteLine("Failed updating item");
                return (false, "Failed to update this item");
            }
        }
    }
}