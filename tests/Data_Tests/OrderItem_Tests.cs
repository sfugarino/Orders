using core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Data_Tests
{
    public class OrderItem_Tests : BaseTest
    {
        readonly Order _order = new Order();
        public OrderItem_Tests() : base() 
        {
            DbContext.Orders.Add(_order);
            DbContext.SaveChanges();
        }

        [Fact]
        public void OrderItemTableShouldBeCreated()
        {
            Assert.False(DbContext.OrderItems.Any());
        }

        [Fact]
        public void AddedOrderItemShouldGetGeneratedId()
        {
            var newOrderItem = new OrderItem();
            newOrderItem.OrderId = _order.Id;
            DbContext.OrderItems.Add(newOrderItem);
            DbContext.SaveChanges();

            Assert.NotEqual(Guid.Empty, newOrderItem.Id);
        }

        [Fact]
        public void AddedOrderItemShouldGetPersisted()
        {
            var newOrderItem = new OrderItem();
            newOrderItem.OrderId = _order.Id;
            DbContext.OrderItems.Add(newOrderItem);
            DbContext.SaveChanges();

            Assert.Equal(newOrderItem, DbContext.OrderItems.Find(newOrderItem.Id));
            Assert.Equal(1, DbContext.Orders.Count());
        }

        [Fact]
        public void OrderShouldContainOrderItem()
        {
            var newOrderItem = new OrderItem();
            newOrderItem.OrderId = _order.Id;
            DbContext.OrderItems.Add(newOrderItem);
            DbContext.SaveChanges();

            var orderItems = DbContext.OrderItems.Where(o => o.OrderId == _order.Id);
            Assert.Equal(1, orderItems.Count());
            Assert.Equal(newOrderItem.Id, orderItems.First().Id);
        }
    }
}
