using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWatcher.Entities;
using MyWatcher.Models;
using MyWatcher.Services;

namespace MyWatcherApi.Api
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("getAll/{serviceId}")]
        public async Task<List<ItemGetDTO>> GetAllItems(int serviceId)
        {
            return await _itemService.GetAllItemsOfService(serviceId);
        }

        [HttpGet("getAllFromUser")]
        public async Task<List<ItemGetDTO>> GetALlItemsFromUser(int serviceId, int userId)
        {
            return await _itemService.GetAllItemsOfServiceFromUser(serviceId, userId);
        }

        [HttpPatch("updateItem")]
        public async Task<IActionResult> UpdateItem([FromBody] ItemUpdateDTO dto)
        {
            await _itemService.UpdateItem(dto);
            return NoContent();
        }
    }
}