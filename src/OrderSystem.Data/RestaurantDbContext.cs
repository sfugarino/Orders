using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OrderSystem.Data.Entities;

namespace OrderSystem.Data
{
    public class RestaurantDbContext: DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ItemSize> ItemSizes { get; set; }
        public DbSet<MenuItem_Size> MenuItem_Sizes { get; set; }


        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MenuItem>()
                 .Property(m => m.Id)
                 .ValueGeneratedOnAdd();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items);

            modelBuilder.Entity<ItemSize>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MenuItem>()
                .HasMany(m => m.Sizes)
                .WithOne(s => s.MenuItem);

            modelBuilder.Entity<MenuItem_Size>()
                .HasKey(ms => new { ms.MenuItemId, ms.ItemSizeId });

            modelBuilder.Entity<MenuItem_Size>()
                .HasOne(m => m.ItemSize);

            modelBuilder.Entity<MenuItem_Size>()
                .HasOne(m => m.MenuItem);

            RestaurantDbInitializer.InitializeDB(modelBuilder);

        }
    }
}