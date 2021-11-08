using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MyWatcherApi.Hubs;
using MyWatcher.Models;
using MyWatcher.Models.Enums;
using MyWatcher.Models.UserItem;
using MyWatcher.Services;
using Serilog;

namespace MyWatcherApi.Api
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UserStockItemController : ControllerBase
    {
        private readonly IUserStockItemService _userStockItemService;
        private readonly IStockItemService _stockItemService;
        private readonly ClientHub _clientHub;
        private readonly ServerHub _serverHub;
        
        public UserStockItemController(IUserStockItemService userStockItemService,
            IStockItemService stockItemService,
            ClientHub clientHub,
            ServerHub serverHub) 
        {
            _userStockItemService = userStockItemService;
            _stockItemService = stockItemService;
            _clientHub = clientHub;
            _serverHub = serverHub;
        }

        [HttpGet("test")] //api/useritem/test
        public async Task<IActionResult> Test()
        {
            return Ok();
        }

        [EnableCors]
        [HttpGet("getStock/{userId}")]
        public async Task<List<UserItemTableDTO>> GetUserItems(Guid userId)
        {
            return await _userStockItemService.GetUsersItemsFromService(userId, Service.Stock);
        }

        [HttpPost("add")]
        public async Task<IActionResult> PostUserItem([FromBody] UserItemAddDTO dto)
        {
            var id = await _userStockItemService.AddUserItem(dto);
            if (id == null)
            {
                return new ConflictResult();
            }
            Log.Information($"Added UserItem with Id {id}");
            return new OkObjectResult(id);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserItem([FromBody] UserItemDeleteDTO dto)
        {
            var success = await _userStockItemService.DeleteUserItem(dto);
            if (!success) return new ConflictResult();
            return NoContent();
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUserItem([FromBody] UserItemUpdateDTO dto)
        {
            var success = await _userStockItemService.UpdateUserItem(dto);
            if (!success) return new ConflictResult();
            return NoContent();
        }

        [HttpPatch("forceRescan")]
        public async Task<IActionResult> ForceRescan([FromBody] ForceRescanRequest request)
        {
            //todo implement so we know when database is online and return according to this
            await _serverHub.StartUserScraping(request);
            return NoContent();
        }
    }
}