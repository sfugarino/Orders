using core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace data
{
    public class RestaurantDbContext: DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
            
        }
    }
}