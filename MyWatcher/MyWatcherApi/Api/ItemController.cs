using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWatcher.Entities;
using MyWatcher.Models;
using MyWatcher.Models.Item;
using MyWatcher.Services;
using MyWatcherApi.Hubs;

namespace MyWatcherApi.Api
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ClientHub _clientHub;

        public ItemController(IItemService itemService, ClientHub clientHub)
        {
            _itemService = itemService;
            _clientHub = clientHub;
        }

        [HttpGet("getAll/{serviceId}")]
        public async Task<List<ItemGetDTO>> GetAllItems(int serviceId)
        {
            return await _itemService.GetAllItemsOfService(serviceId);
        }

        [HttpGet("getAllFromUser")]
        public async Task<List<ItemGetDTO>> GetAllItemsFromUser(int serviceId, int userId)
        {
            return await _itemService.GetAllItemsOfServiceFromUser(serviceId, userId);
        }

        [HttpGet("getFromUserNotRecentlyUpdated/{serviceId}/{userId}")]
        public async Task<List<ItemGetDTO>> GetItemsFromUserNotRecentlyUpdated(int serviceId, int userId)
        {
            return await _itemService.GetAllItemsOfServiceFromUserNotUpdatedLastHour(serviceId, userId);
        }

        [HttpPatch("updateItem")]
        public async Task<IActionResult> UpdateItem([FromBody] ItemUpdateDTO dto)
        {
            await _itemService.UpdateItem(dto);
            return NoContent();
        }
        
        [HttpPost("scrapingCompleted")]
        public async Task<IActionResult> ScrapingCompleted([FromBody] ScrapingCompleteDTO dto)
        {
            await _clientHub.ScrapingFinished(dto);
            return NoContent();
        }
    }
}