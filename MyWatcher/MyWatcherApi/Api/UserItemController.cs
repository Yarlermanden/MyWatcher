using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWatcher.Models;
using MyWatcher.Services;

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

        [HttpGet("get/{userId}")]
        public async Task<List<UserItemTableDTO>> GetUserItems(int userId)
        {
            return await _userItemService.GetUsersItemsFromService(userId, 1);
        }
        
        
    }
}