using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyWatcher.Entities;
using MyWatcher.Models;
using MyWatcher.Models.Enums;
using MyWatcher.Models.Item;

namespace MyWatcher.Services
{
    public interface IItemService
    {
        public Task<Item?> GetItem(Guid id);
        public Task<Item?> GetItemFromUrlAndServiceId(string url, Service service);
        public Task<Guid> AddItem(string url, Service service);
        public Task UpdateItem(ItemUpdateDTO dto);
        public Task<List<ItemGetDTO>> GetAllItemsOfService(Service service);
        public Task<List<ItemGetDTO>> GetAllItemsOfServiceFromUser(Service service, Guid userId);
        public Task<List<ItemGetDTO>> GetAllItemsOfServiceFromUserNotUpdatedLastHour(Service service, Guid userId);
    }
    
    public class ItemService : IItemService
    {
        private readonly DatabaseContext _dbContext;
        
        public ItemService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Item?> GetItem(Guid id)
        {
            try
            {
                return await _dbContext.StockItems.FindAsync(id);
            }
            catch (Exception e)
            {
                //Log.Error($"Failed finding item with id: {id}: {e.Message}");
                return null;
            }
        }

        public async Task<Item?> GetItemFromUrlAndServiceId(string url, Service service)
        {
            var item = await _dbContext.StockItems.Where(i => i.URL == url && i.Service == service).
                FirstOrDefaultAsync();
            return item;
        }

        public async Task<Guid> AddItem(string url, Service service)
        {
            Item item;
            switch (service)
            {
                case Service.Stock:
                    item = new StockItem(url, service);
                    await _dbContext.StockItems.AddAsync((StockItem)item);
                    break;
                case Service.SecondHand:
                    item = new SecondHandItem(url, service);
                    await _dbContext.SecondHandItems.AddAsync((SecondHandItem) item);
                    break;
                default:
                    item = new Item();
                    break;
            }
            await _dbContext.SaveChangesAsync();
            return item.Id;
        }

        public async Task UpdateItem(ItemUpdateDTO dto)
        {
            var item = await _dbContext.StockItems.FindAsync(dto.Id);
            if (item == null || dto.Price == 0) return;
            item.Price = dto.Price;
            if (item.PriceThisWeek == 0) item.PriceThisWeek = item.Price;
            item.LastScan = DateTime.UtcNow;
            if (item.Price < item.PriceLowestKnown || item.PriceLowestKnown == 0) item.PriceLowestKnown = item.Price;
            if ((item.LastScan - item.LastWeeklyPriceUpdate).Value > TimeSpan.FromDays(7))
            {
                item.PriceLastWeek = item.PriceThisWeek;
                item.PriceThisWeek = item.Price;
                item.LastWeeklyPriceUpdate = item.LastScan;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ItemGetDTO>> GetAllItemsOfService(Service service)
        {
            var items = await _dbContext.StockItems
                .Where(i => i.Service == service)
                .Select(i => new ItemGetDTO(i))
                .ToListAsync();
            return items;
        }

        public async Task<List<ItemGetDTO>> GetAllItemsOfServiceFromUser(Service service, Guid userId)
        {
            var items = await GetAllActiveItemsOfServiceAndUser(service, userId);
            return await items.Select(ui => new ItemGetDTO(ui.StockItem)).ToListAsync();
        }

        public async Task<List<ItemGetDTO>> GetAllItemsOfServiceFromUserNotUpdatedLastHour(Service service, Guid userId)
        {
            var oneHourAgo = DateTime.UtcNow-TimeSpan.FromHours(1);
            var items = await GetAllActiveItemsOfServiceAndUser(service, userId);
            return await items.Where(ui => ui.StockItem.LastScan < oneHourAgo || ui.StockItem.LastScan == null)
                .Select(ui => new ItemGetDTO(ui.StockItem)).ToListAsync();
        }

        private async Task<IQueryable<UserStockItem>> GetAllActiveItemsOfServiceAndUser(Service service, Guid userId)
        {
            var items = _dbContext.UserStockItems.Include(ui => ui.StockItem)
                .Where(ui => ui.UserId == userId && ui.StockItem.Service == service && ui.Active);
            return items;
        }
    }
}