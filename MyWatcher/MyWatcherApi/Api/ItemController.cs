using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWatcher.Entities;
using MyWatcher.Models;
using MyWatcher.Models.Enums;
using MyWatcher.Models.Item;
using MyWatcher.Services;
using MyWatcherApi.Hubs;

namespace MyWatcherApi.Api
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IStockItemService _stockItemService;
        private readonly ClientHub _clientHub;

        public ItemController(IStockItemService stockItemService, ClientHub clientHub)
        {
            _stockItemService = stockItemService;
            _clientHub = clientHub;
        }

        [HttpGet("getAll/{serviceId}")]
        public async Task<List<ItemGetDTO>> GetAllItems(Service service)
        {
            return await _stockItemService.GetAllItemsOfService(service);
        }

        [HttpGet("getAllFromUser")]
        public async Task<List<ItemGetDTO>> GetAllItemsFromUser(Service service, Guid userId)
        {
            return await _stockItemService.GetAllItemsOfServiceFromUser(service, userId);
        }

        [HttpGet("getFromUserNotRecentlyUpdated/{serviceId}/{userId}")]
        public async Task<List<ItemGetDTO>> GetItemsFromUserNotRecentlyUpdated(Service service, Guid userId)
        {
            return await _stockItemService.GetAllItemsOfServiceFromUserNotUpdatedLastHour(service, userId);
        }

        [HttpPatch("updateItem")]
        public async Task<IActionResult> UpdateItem([FromBody] ItemUpdateDTO dto)
        {
            await _stockItemService.UpdateItem(dto);
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