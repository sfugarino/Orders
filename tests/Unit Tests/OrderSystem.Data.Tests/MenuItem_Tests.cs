using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrderSystem.Data.Tests
{
    public class MenuItem_Tests : BaseTest
    {
        [Fact]
        public void Given_Empty_Database_MenuItem_Table_Should_Be_Created()
        {
            Assert.True(DbContext.MenuItems.Any());
        }

        [Fact]
        public void Given_MenuItem_Added_Id_Should_Be_Generated()
        {
            int expected = GetLastMenuItemId() + 1;

            var newMenuItem = new MenuItem();
            newMenuItem.Name = "Quarter Pounder";
            newMenuItem.Station = Station.Grill;
            newMenuItem.Sizes = new List<MenuItem_Size>() { new MenuItem_Size { ItemSizeId = 1, Price = 3.50M} };
            DbContext.MenuItems.Add(newMenuItem);
            DbContext.SaveChanges();

            Assert.Equal(expected, newMenuItem.Id);

        }

        [Fact]
        public void Given_MenuItem_Added_With_Size_MenuItem_Size_Table_Should_Be_Populated()
        {
            int expectedId = GetLastMenuItemId() + 1;

            var newMenuItem = new MenuItem();
            newMenuItem.Name = "Quarter Pounder";
            newMenuItem.Station = Station.Grill;
            newMenuItem.Sizes = new List<MenuItem_Size>() { new MenuItem_Size { ItemSizeId = 1, Price = 3.50M } };
            DbContext.MenuItems.Add(newMenuItem);
            DbContext.SaveChanges();

            Assert.Equal(expectedId, newMenuItem.Id);

            int expectedMenuItemId = newMenuItem.Id;
            int expectedItemSizeId = newMenuItem.Sizes.First().ItemSizeId;
            decimal expectedPrice = newMenuItem.Sizes.First().Price;

            var size = DbContext.MenuItem_Sizes.FirstOrDefault(i => i.MenuItemId == expectedMenuItemId);

            Assert.NotNull(size);
            Assert.Equal(expectedItemSizeId, size.ItemSizeId);
            Assert.Equal(expectedPrice, size.Price);

        }
    }
}
