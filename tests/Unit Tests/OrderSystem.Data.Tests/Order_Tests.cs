using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OrderSystem.Data.Entities;

namespace OrderSystem.Data.Tests
{
    public class Order_Tests : BaseTest
    {
        [Fact]
        public void Given_Empty_Database_Order_Table_Should_Be_Created()
        {
            Assert.False(DbContext.Orders.Any());
        }

        [Fact]
        public void Given_Order_Added_Id_Should_Be_Generated()
        {
            int expected = GetLastOrderId() + 1;

            var newOrder = new Order();
            newOrder.ServerId = 1;
            DbContext.Orders.Add(newOrder);
            DbContext.SaveChanges();

            Assert.Equal(expected, newOrder.Id);
        }

        [Fact]
        public void Given_Order_Added_Order_Should_Be_Persisted()
        {
            int expected = GetLastOrderId() + 1;

            var newOrder = new Order();
            newOrder.ServerId = 1;
            DbContext.Orders.Add(newOrder);
            DbContext.SaveChanges();

            Assert.Equal(newOrder, DbContext.Orders.Find(newOrder.Id));
            Assert.Equal(expected, DbContext.Orders.Count());
        }
    }
}
