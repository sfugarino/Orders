using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using OrderSystem.Data.Entities;

namespace OrderSystem.Data
{
    public class RestaurantDbContext: DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(ol => ol.Items);
        }
    }
}