using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWatcher.Services;

namespace MyWatcherApi.Api
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ItemController
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        
    }
}