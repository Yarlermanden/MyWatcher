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
    public class UserItemController : ControllerBase
    {
        private readonly IUserItemService _userItemService;
        private readonly IItemService _itemService;
        private readonly ClientHub _clientHub;
        private readonly ServerHub _serverHub;
        
        public UserItemController(IUserItemService userItemService,
            IItemService itemService,
            ClientHub clientHub,
            ServerHub serverHub) 
        {
            _userItemService = userItemService;
            _itemService = itemService;
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
        public async Task<List<UserItemTableDTO>> GetUserItems(int userId)
        {
            return await _userItemService.GetUsersItemsFromService(userId, Service.Stock);
        }

        [HttpPost("add")]
        public async Task<IActionResult> PostUserItem([FromBody] UserItemAddDTO dto)
        {
            var id = await _userItemService.AddUserItem(dto);
            if (id == -1)
            {
                return new ConflictResult();
            }
            Log.Information($"Added UserItem with Id {id}");
            return new OkObjectResult(id);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserItem([FromBody] UserItemDeleteDTO dto)
        {
            var success = await _userItemService.DeleteUserItem(dto);
            if (!success) return new ConflictResult();
            return NoContent();
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUserItem([FromBody] UserItemUpdateDTO dto)
        {
            var success = await _userItemService.UpdateUserItem(dto);
            if (!success) return new ConflictResult();
            return NoContent();
        }

        [HttpPatch("forceRescan")]
        public async Task<IActionResult> ForceRescan([FromBody] ForceRescanRequest request)
        {
            //var success  = await _scraperSocketService.StartScrapingOfUserItems(request);
            //var success = isServerOnline
            await _serverHub.StartUserScraping(request);
            //if (!success) { return new ConflictResult(); }
            return NoContent();
        }
    }
}