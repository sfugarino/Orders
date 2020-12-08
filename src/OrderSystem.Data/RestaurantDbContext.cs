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


        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(ol => ol.Items);

            modelBuilder.Entity<Order>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<MenuItem>()
                 .Property(e => e.Id)
                 .ValueGeneratedOnAdd();


            modelBuilder.Entity<Employee>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Employee>().HasData(
                new Employee() { Id = 1, FirstName = "Bunn", MiddleName = "E", LastName = "Carlos" },
                new Employee() { Id = 2, FirstName = "Ronald",LastName = "McDonald" }
            );

            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem() { Id = 1, Name = "Big Mac", Station = Station.Grill, Price = 3.99M },
                new MenuItem() { Id = 2, Name = "Large Fries", Station = Station.Grill, Price = 1.89M },
                new MenuItem() { Id = 3, Name = "Medium Fries", Station = Station.Grill, Price = 1.79M },
                new MenuItem() { Id = 4, Name = "Smal Fries", Station = Station.Grill, Price = 1.39M },
                new MenuItem() { Id = 5, Name = "Large Drink", Station = Station.Grill, Price = 1.00M },
                new MenuItem() { Id = 6, Name = "Medium Drink", Station = Station.Grill, Price = 1.00M },
                new MenuItem() { Id = 7, Name = "Small Drink", Station = Station.Grill, Price = 1.00M }
            );
        }
    }
}