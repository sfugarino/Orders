using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OrderSystem.Data.Entities;

namespace OrderSystem.Data.Tests
{
    public class OrderItem_Tests : BaseTest
    {
        readonly Order _order = new Order();
        public OrderItem_Tests() : base() 
        {
            _order.ServerId = 1;
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
            int expected = GetLastOrderItemId() + 1;
            var newOrderItem = new OrderItem();
            newOrderItem.OrderId = _order.Id;
            newOrderItem.MenuItemId = GetLastMenuItemId();
            DbContext.OrderItems.Add(newOrderItem);
            DbContext.SaveChanges();

           Assert.Equal(expected, newOrderItem.Id);
        }

        [Fact]
        public void AddedOrderItemShouldGetPersisted()
        {
            int orderId = GetLastOrderId();
            int expected = GetLastOrderItemId() + 1;

            var newOrderItem = new OrderItem();
            newOrderItem.OrderId = _order.Id;
            newOrderItem.MenuItemId = GetLastMenuItemId();
            DbContext.OrderItems.Add(newOrderItem);
            DbContext.SaveChanges();

            Assert.Equal(newOrderItem, DbContext.OrderItems.Find(newOrderItem.Id));
            Assert.Equal(expected, DbContext.Orders.Count());
        }

        [Fact]
        public void OrderShouldContainOrderItem()
        {
            int expected = GetLastOrderItemId() + 1;

            var newOrderItem = new OrderItem();
            newOrderItem.OrderId = _order.Id;
            newOrderItem.MenuItemId = GetLastMenuItemId();
            DbContext.OrderItems.Add(newOrderItem);
            DbContext.SaveChanges();

            var orderItems = DbContext.OrderItems.Where(o => o.OrderId == _order.Id);
            Assert.Equal(expected, orderItems.Count());
            Assert.Equal(expected, newOrderItem.Id);
        }
    }
}
