using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MyWatcher.Models;
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
        
        public UserItemController(IUserItemService userItemService,
            IItemService itemService) 
        {
            _userItemService = userItemService;
            _itemService = itemService;
        }

        [HttpGet("test")] //api/useritem/test
        public async Task<IActionResult> Test()
        {
            return Ok();
        }

        [EnableCors]
        [HttpGet("getpricerunner/{userId}")]
        public async Task<List<UserItemTableDTO>> GetUserItems(int userId)
        {
            return await _userItemService.GetUsersItemsFromService(userId, 1);
        }

        [HttpPost("add")]
        public async Task<IActionResult> PostUserItem([FromBody] UserItemAddDTO dto)
        {
            var id = await _userItemService.AddUserItem(dto);
            Log.Information($"Added UserItem with Id {id}");
            return new OkObjectResult(id);
            //Todo return id - update frontend item with this id
            //return NoContent();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserItem([FromBody] UserItemDeleteDTO dto)
        {
            var success = await _userItemService.DeleteUserItem(dto);
            //Todo return based on success - tell use if they tried to delete invalid item
            return NoContent();
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUserItem([FromBody] UserItemUpdateDTO dto)
        {
            var success = await _userItemService.UpdateUserItem(dto);
            //Todo return based on success
            return NoContent();
        }
    }
}