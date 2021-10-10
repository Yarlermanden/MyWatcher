using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyWatcher.Entities;
using MyWatcher.Models;

namespace MyWatcher.Services
{
    public interface IUserItemService
    {
        public Task<int> AddUserItem(int userId, string url, int serviceId, string name);
        public Task<int> AddUserItem(int userId, int itemId, string name);
        public Task<UserItem?> GetUserItem(int userId, int itemId);
        public Task<List<UserItemTableDTO>> GetUsersItemsFromService(int userId, int serviceId);
    }
    
    public class UserItemService : IUserItemService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbFactory;
        private readonly IItemService _itemService;

        public UserItemService(IDbContextFactory<DatabaseContext> dbFactory,
            IItemService itemService)
        {
            _dbFactory = dbFactory;
            _itemService = itemService;
        }

        public async Task<int> AddUserItem(int userId, string url, int serviceId, string name)
        {
            var item = await _itemService.GetItemFromUrlAndServiceId(url, serviceId);
            if (item == null)
            {
                var id = await _itemService.AddItem(url, serviceId);
                item = await _itemService.GetItem(id);
            }

            //check if this userItem already exists
            var userItem = await GetUserItem(userId, item.Id);
            if (userItem != null) return userItem.Id;

            return await AddUserItem(userId, item.Id, name);
        }

        public async Task<int> AddUserItem(int userId, int itemId, string name)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();

            var userItem = new UserItem(userId, itemId, name);
            await dbContext.UserItems.AddAsync(userItem);
            await dbContext.SaveChangesAsync();
            return userItem.Id;
        }

        public async Task<UserItem?> GetUserItem(int userId, int itemId)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var userItem = await dbContext.UserItems.Where(ui => ui.UserId == userId && ui.ItemId == itemId)
                .FirstOrDefaultAsync();
            return userItem;
        }

        public async Task<List<UserItemTableDTO>> GetUsersItemsFromService(int userId, int serviceId)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.UserItems.Include(ui => ui.Item)
                .Where(ui => ui.UserId == userId && ui.Item.ServiceId == serviceId)
                .Select(ui => new UserItemTableDTO()
                {
                    Id = ui.Id,
                    Name = ui.Name,
                    Price = ui.Item.Price,
                    LastScan = ui.Item.LastScan
                })
                .ToListAsync();
        }
    }
}