using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyWatcher.Entities;
using MyWatcher.Models;
using MyWatcher.Models.Enums;
using MyWatcher.Models.UserItem;

namespace MyWatcher.Services
{
    public interface IUserItemService
    {
        public Task<Guid?> AddUserItem(UserItemAddDTO dto);
        public Task<Guid?> AddUserItem(Guid userId, Guid itemId, string name);
        public Task<UserStockItem?> GetUserItem(Guid userId, Guid itemId);
        public Task<List<UserItemTableDTO>> GetUsersItemsFromService(Guid userId, Service service);
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

        public async Task<Guid?> AddUserItem(UserItemAddDTO dto)
        {
            var item = await _itemService.GetItemFromUrlAndServiceId(dto.URL, dto.Service);
            if (item == null)
            {
                var id = await _itemService.AddItem(dto.URL, dto.Service);
                item = await _itemService.GetItem(id);
            }

            //check if this userItem already exists
            var userItem = await GetUserItem(dto.UserId, item.Id);
            if (userItem != null) return null;

            return await AddUserItem(dto.UserId, item.Id, dto.Name);
        }

        public async Task<Guid?> AddUserItem(Guid userId, Guid itemId, string name)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();

            var userItem = new UserStockItem(userId, itemId, name);
            await dbContext.UserStockItems.AddAsync(userItem);
            await dbContext.SaveChangesAsync();
            return userItem.Id;
        }

        public async Task<UserStockItem?> GetUserItem(Guid userId, Guid itemId)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var userItem = await dbContext.UserStockItems.Where(ui => ui.UserId == userId && ui.StockItemId == itemId)
                .FirstOrDefaultAsync();
            return userItem;
        }

        public async Task<List<UserItemTableDTO>> GetUsersItemsFromService(Guid userId, Service service)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            return await dbContext.UserStockItems.Include(ui => ui.StockItem)
                .Where(ui => ui.UserId == userId && ui.StockItem.Service == service)
                .Select(ui => new UserItemTableDTO()
                {
                    Id = ui.Id,
                    Name = ui.Name,
                    Price = ui.StockItem.Price,
                    PriceComparedToLastWeek = ui.StockItem.PricePercentageCalculator(ui.StockItem.Price, ui.StockItem.PriceLastWeek),
                    LowestPriceKnown = ui.StockItem.PriceLowestKnown,
                    LastScan = ui.StockItem.LastScan,
                    Active = ui.Active,
                })
                .OrderByDescending(ui => ui.Id)
                .ToListAsync();
        }

        public async Task<bool> DeleteUserItem(UserItemDeleteDTO dto)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var userItem = await dbContext.UserStockItems.FindAsync(dto.Id);
            if (userItem != null && userItem.UserId == dto.UserId)
            {
                dbContext.UserStockItems.Remove(userItem);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateUserItem(UserItemUpdateDTO dto)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var userItem = await dbContext.UserStockItems.FindAsync(dto.Id);
            if (userItem == null || userItem.UserId != dto.UserId) return false;
            userItem.Active = dto.Active;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}