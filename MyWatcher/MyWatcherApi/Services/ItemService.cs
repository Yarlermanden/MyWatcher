using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyWatcher.Entities;
using MyWatcher.Models;

namespace MyWatcher.Services
{
    public interface IItemService
    {
        public Task<Item?> GetItem(int id);
        public Task<Item?> GetItemFromUrlAndServiceId(string url, int serviceId);
        public Task<int> AddItem(string url, int serviceId);
        public Task UpdateItem(ItemUpdateDTO dto);
        public Task<List<ItemGetDTO>> GetAllItemsOfService(int serviceId);
        public Task<List<ItemGetDTO>> GetAllItemsOfServiceFromUser(int serviceId, int userId);
    }
    
    public class ItemService : IItemService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbFactory;
        
        public ItemService(IDbContextFactory<DatabaseContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<Item?> GetItem(int id)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            try
            {
                return await dbContext.Items.FindAsync(id);
            }
            catch (Exception e)
            {
                //Log.Error($"Failed finding item with id: {id}: {e.Message}");
                return null;
            }
        }

        public async Task<Item?> GetItemFromUrlAndServiceId(string url, int serviceId)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var item = await dbContext.Items.Where(i => i.URL == url && i.ServiceId == serviceId).
                FirstOrDefaultAsync();
            return item;
        }

        public async Task<int> AddItem(string url, int serviceId)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var item = new Item(url, serviceId);
            
            await dbContext.Items.AddAsync(item);
            await dbContext.SaveChangesAsync();
            return item.Id;
        }

        public async Task UpdateItem(ItemUpdateDTO dto)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var item = await dbContext.Items.FindAsync(dto.Id);
            if (item == null || dto.Price == 0) return;
            item.Price = dto.Price;
            item.LastScan = DateTime.UtcNow;
            if (item.Price < item.PriceLowestKnown || item.PriceLowestKnown == 0) item.PriceLowestKnown = item.Price;
            if ((item.LastScan - item.LastWeeklyPriceUpdate).Value > TimeSpan.FromDays(7))
            {
                item.PriceLastWeek = item.PriceThisWeek;
                item.PriceThisWeek = item.Price;
                item.LastWeeklyPriceUpdate = item.LastScan;
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<ItemGetDTO>> GetAllItemsOfService(int serviceId)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var items = await dbContext.Items
                .Where(i => i.ServiceId == serviceId)
                .Select(i => new ItemGetDTO(i))
                .ToListAsync();
            return items;
        }

        public async Task<List<ItemGetDTO>> GetAllItemsOfServiceFromUser(int serviceId, int userId)
        {
            await using var dbContext = await _dbFactory.CreateDbContextAsync();
            var items = await dbContext.UserItems.Include(ui => ui.Item)
                .Where(ui => ui.UserId == userId && ui.Item.ServiceId == serviceId)
                .Select(ui => new ItemGetDTO(ui.Item))
                .ToListAsync();
            return items;
        }
    }
}