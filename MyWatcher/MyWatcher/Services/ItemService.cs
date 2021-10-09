using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyWatcher.Entities;
using Serilog;

namespace MyWatcher.Services
{
    public interface IItemService
    {
        public Task<Item?> GetItem(int id);
        public Task<Item?> GetItemFromUrlAndServiceId(string url, int serviceId);
        public Task<int> AddItem(string url, int serviceId);
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
                Log.Error($"Failed finding item with id: {id}: {e.Message}");
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
    }
}