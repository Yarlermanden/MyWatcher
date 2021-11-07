using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyWatcher.Entities;

namespace MyWatcher.Services
{
    public class DatabaseContext : DbContext
    {
        //public DbSet<Item> Items { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<SecondHandItem> SecondHandItems { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<UserItem> UserItems { get; set; }
        public DbSet<UserStockItem> UserStockItems { get; set; }
        public DbSet<UserSecondHandItem> UserSecondHandItems { get; set; }
        public DbSet<WebsiteStockItem> WebsiteStockItems { get; set; }
        public DbSet<WebsiteSecondHandItem> WebsiteSecondHandItems { get; set; }
        public DbSet<Website> Websites { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Continent> Continents { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Debug.WriteLine($"{ContextId} context created.");
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //var items = builder.Entity<Item>().ToTable("Item");
            var stockItems = builder.Entity<StockItem>().ToTable("StockItem");
            var secondHandItems = builder.Entity<SecondHandItem>().ToTable("SecondHandItem");
            var users = builder.Entity<User>().ToTable("User");
            //var userItems = builder.Entity<UserItem>().ToTable("UserItem");
            var userStockItems = builder.Entity<UserStockItem>().ToTable("UserStockItem");
            var userSecondHandItems = builder.Entity<UserSecondHandItem>().ToTable("UserSecondHandItem");
            var websiteStockItems = builder.Entity<WebsiteStockItem>().ToTable("WebsiteStockItem");
            var websiteSecondHandItems = builder.Entity<WebsiteSecondHandItem>().ToTable("WebsiteSecondHandItem");
            var websites = builder.Entity<Website>().ToTable("Website");
            var countries = builder.Entity<Country>().ToTable("Country");
            var continents = builder.Entity<Continent>().ToTable("Continent");

            stockItems.HasKey(item => item.Id);
            stockItems.Property(item => item.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            secondHandItems.HasKey(item => item.Id);
            secondHandItems.Property(item => item.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            
            users.HasKey(user => user.Id);
            users.Property(user => user.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            userStockItems.HasKey(ui => ui.Id);
            userStockItems.Property(ui => ui.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            userStockItems.HasOne(ui => ui.StockItem);
            userStockItems.HasOne(ui => ui.User);
            
            userSecondHandItems.HasKey(ui => ui.Id);
            userSecondHandItems.Property(ui => ui.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            userSecondHandItems.HasOne(ui => ui.SecondHandItem);
            userSecondHandItems.HasOne(ui => ui.User);

            websiteStockItems.HasKey(w => w.Id);
            websiteStockItems.Property(w => w.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            websiteStockItems.HasOne(w => w.StockItem);
            websiteStockItems.HasOne(w => w.Website);
            
            websiteSecondHandItems.HasKey(w => w.Id);
            websiteSecondHandItems.Property(w => w.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            websiteSecondHandItems.HasOne(w => w.SecondHandItem);
            websiteSecondHandItems.HasOne(w => w.Website);

            websites.HasKey(w => w.Id);
            websites.Property(w => w.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            websites.HasOne(w => w.Country);

            countries.HasKey(c => c.Id);
            countries.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            countries.HasOne(c => c.Continent);

            continents.HasKey(c => c.Id);
            continents.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();
        }

        /// <summary>
        ///     Dispose pattern.
        /// </summary>
        public override void Dispose()
        {
            Debug.WriteLine($"{ContextId} context disposed.");
            base.Dispose();
        }
        
        /// <summary>
        ///     Dispose pattern.
        /// </summary>
        /// <returns>A <see cref="ValueTask" /></returns>
        public override ValueTask DisposeAsync()
        {
            Debug.WriteLine($"{ContextId} context disposed async.");
            return base.DisposeAsync();
        }
    }
}