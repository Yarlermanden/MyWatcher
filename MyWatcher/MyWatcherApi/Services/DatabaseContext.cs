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
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<UserItem> UserItems { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Debug.WriteLine($"{ContextId} context created.");
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var items = builder.Entity<Item>().ToTable("Item");
            var users = builder.Entity<User>().ToTable("User");
            var services = builder.Entity<Service>().ToTable("Service");
            var userItems = builder.Entity<UserItem>().ToTable("UserItem");

            items.HasKey(item => item.Id);
            items.Property(item => item.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            users.HasKey(user => user.Id);
            users.Property(user => user.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            services.HasKey(service => service.Id);
            services.Property(service => service.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            services.HasMany(s => s.Items).WithOne(i => i.Service).HasForeignKey(i => i.ServiceId);

            userItems.HasKey(ui => ui.Id);
            userItems.Property(ui => ui.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            userItems.HasOne(ui => ui.Item);
            userItems.HasOne(ui => ui.User);

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