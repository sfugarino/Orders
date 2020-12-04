using core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Data_Tests
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
            var newOrder = new Order();
            DbContext.Orders.Add(newOrder);
            DbContext.SaveChanges();

            Assert.NotEqual(Guid.Empty, newOrder.Id);
        }

        [Fact]
        public void AddedOrderShouldGetPersisted()
        {
            var newOrder = new Order();
            DbContext.Orders.Add(newOrder);
            DbContext.SaveChanges();

            Assert.Equal(newOrder, DbContext.Orders.Find(newOrder.Id));
            Assert.Equal(1, DbContext.Orders.Count());
        }
    }
}
