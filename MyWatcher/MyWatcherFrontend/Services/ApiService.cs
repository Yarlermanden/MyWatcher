using System;
using System.IO;
using System.Collections.Generic;
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
        public Task<(bool, int)> AddUserItem(UserItemAddDTO dto);
        public Task<bool> DeleteUserItem(UserItemDeleteDTO dto);
        public Task<bool> UpdateUserItem(UserItemUpdateDTO dto);
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

        public async Task<(bool, int)> AddUserItem(UserItemAddDTO dto)
        {
            var response = await _communicationService.PostItemRequest($"/api/useritem/add", dto);
            if (response.IsSuccessStatusCode)
            {
                var id = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Successfully posted Item with id: {id}"); 
                
                return (response.IsSuccessStatusCode, int.Parse(id));
            }
            else
            {
                Console.WriteLine("Failed posting Item");
                return (response.IsSuccessStatusCode, -1);
            }
        }

        public async Task<bool> DeleteUserItem(UserItemDeleteDTO dto)
        {
            var response = await _communicationService.DeleteItemRequest($"/api/useritem/delete", dto);
            if(response.IsSuccessStatusCode) {Console.WriteLine("Succesfully deleted item"); }
            else { Console.WriteLine("Failed posting Item"); }
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserItem(UserItemUpdateDTO dto)
        {
            var response = await _communicationService.UpdateItemRequest($"/api/useritem/update", dto);
            if(response.IsSuccessStatusCode) {Console.WriteLine("Successfully updated Item");}
            else{Console.WriteLine("Failed updating item");}
            return response.IsSuccessStatusCode;
        }
    }
}