using Microsoft.EntityFrameworkCore;
using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data
{
    public sealed class RestaurantDbInitializer
    {
        public static void InitializeDB(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemSize>().HasData(
                new ItemSize() { Id = 1, Name = "N/A" },
                new ItemSize() { Id = 2, Name = "Small" },
                new ItemSize() { Id = 3, Name = "Medium" },
                new ItemSize() { Id = 4, Name = "Large" }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee() { Id = 1, FirstName = "Bunn", MiddleName = "E", LastName = "Carlos" },
                new Employee() { Id = 2, FirstName = "Ronald", LastName = "McDonald" }
            );

            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem()
                {
                    Id = 1,
                    Name = "Big Mac",
                    Station = Station.Grill,
                },
                new MenuItem()
                {
                    Id = 2,
                    Name = "Fries",
                    Station = Station.Grill,
                },
                new MenuItem()
                {
                    Id = 7,
                    Name = "Drink",
                    Station = Station.Grill,
                }
            );

            modelBuilder.Entity<MenuItem_Size>().HasData(
                new MenuItem_Size() { MenuItemId = 1, ItemSizeId = 1, Price = 4.50M },
                new MenuItem_Size() { MenuItemId = 2, ItemSizeId = 2, Price = 2.50M },
                new MenuItem_Size() { MenuItemId = 2, ItemSizeId = 3, Price = 2.50M },
                new MenuItem_Size() { MenuItemId = 2, ItemSizeId = 4, Price = 2.50M },
                new MenuItem_Size() { MenuItemId = 7, ItemSizeId = 2, Price = 2.50M },
                new MenuItem_Size() { MenuItemId = 7, ItemSizeId = 3, Price = 2.50M },
                new MenuItem_Size() { MenuItemId = 7, ItemSizeId = 4, Price = 2.50M }
            );
        }
    }
}
