using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MyWatcher.Models;
using System.Text.Json;
using MyWatcher.Entities;
using MyWatcher.Models.User;
using MyWatcher.Models.UserItem;
using MyWatcher.Services;
using Npgsql.Replication;

namespace MyWatcherFrontend.Services
{
    public interface IApiService
    {
        public Task<List<UserItemTableDTO>> GetUserItems(User user);
        public Task<(bool, int, string)> AddUserItem(UserItemAddDTO dto);
        public Task<(bool, string)> DeleteUserItem(UserItemDeleteDTO dto);
        public Task<(bool, string)> UpdateUserItem(UserItemUpdateDTO dto);
        public Task<bool> ForceRescanUserItems(ForceRescanRequest request);
        Task<UserGetDTO?> RegisterUser(UserRegisterDTO dto);
        Task<UserGetDTO?> LoginUser(UserLoginDTO dto);
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
                var errorMessage = "";
                if (response.StatusCode == HttpStatusCode.Conflict) errorMessage = "Item already exists";
                else if (response.StatusCode == HttpStatusCode.ServiceUnavailable) errorMessage = "Couldn't reach API";
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
                var errorMessage = "";
                if (response.StatusCode == HttpStatusCode.Conflict) errorMessage = "Couldn't find item";
                else if (response.StatusCode == HttpStatusCode.ServiceUnavailable) errorMessage = "Couldn't reach API";
                return (false, errorMessage);
            }
        }

        public async Task<(bool, string)> UpdateUserItem(UserItemUpdateDTO dto)
        {
            var response = await _communicationService.PatchRequest($"/api/useritem/update", dto);
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

        public async Task<bool> ForceRescanUserItems(ForceRescanRequest request)
        {
            var response = await _communicationService.PatchRequest($"/api/useritem/forceRescan", request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully forced rescan");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to force rescan");
                return false;
            }
        }

        public async Task<UserGetDTO?> RegisterUser(UserRegisterDTO dto)
        {
            var response = await _communicationService.PostItemRequest($"/api/user/registerUser", dto);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully registered user");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserGetDTO>(content);
            }
            else
            {
                Console.WriteLine("Failed registering the user");
                return null;
            }
        }

        public async Task<UserGetDTO?> LoginUser(UserLoginDTO dto)
        {
            var response = await _communicationService.PostItemRequest($"/api/user/loginUser", dto);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully logged user in");
                var content = await response.Content.ReadAsStreamAsync();
                return JsonSerializer.Deserialize<UserGetDTO>(content);
            }
            else
            {
                Console.WriteLine("Failed to login user");
                return null;
            }

        }
    }
}