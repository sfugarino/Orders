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
        public void OrderTableShouldBeCreated()
        {
            Assert.False(DbContext.Orders.Any());
        }

        [Fact]
        public void AddedOrderShouldGetGeneratedId()
        {
            int expected = GetLastOrderId() + 1;

            var newOrder = new Order();
            newOrder.ServerId = 1;
            DbContext.Orders.Add(newOrder);
            DbContext.SaveChanges();

            Assert.Equal(expected, newOrder.Id);
        }

        [Fact]
        public void AddedOrderShouldGetPersisted()
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
