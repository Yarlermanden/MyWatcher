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
        public Task<int> AddUserItem(UserItemAddDTO dto);
        public Task<int> AddUserItem(int userId, int itemId, string name);
        public Task<UserItem?> GetUserItem(int userId, int itemId);
        public Task<List<UserItemTableDTO>> GetUsersItemsFromService(int userId, int serviceId);
        public Task<bool> DeleteUserItem(UserItemDeleteDTO dto);
        public Task<bool> UpdateUserItem(UserItemUpdateDTO dto);
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

        public async Task<int> AddUserItem(UserItemAddDTO dto)
        {
            var item = await _itemService.GetItemFromUrlAndServiceId(dto.URL, dto.ServiceId);
            if (item == null)
            {
                var id = await _itemService.AddItem(dto.URL, dto.ServiceId);
                item = await _itemService.GetItem(id);
            }

            //check if this userItem already exists
            var userItem = await GetUserItem(dto.UserId, item.Id);
            if (userItem != null) return -1;

            return await AddUserItem(dto.UserId, item.Id, dto.Name);
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
                    PriceComparedToLastWeek = ui.Item.PricePercentageCalculator(ui.Item.Price, ui.Item.PriceLastWeek),
                    LowestPriceKnown = ui.Item.PriceLowestKnown,
                    LastScan = ui.Item.LastScan,
                    Active = ui.Active,
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteUserItem(UserItemDeleteDTO dto)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var userItem = await dbContext.UserItems.FindAsync(dto.Id);
            if (userItem != null && userItem.UserId == dto.UserId)
            {
                dbContext.UserItems.Remove(userItem);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateUserItem(UserItemUpdateDTO dto)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var userItem = await dbContext.UserItems.FindAsync(dto.Id);
            if (userItem == null || userItem.UserId != dto.UserId) return false;
            userItem.Active = dto.Active;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}