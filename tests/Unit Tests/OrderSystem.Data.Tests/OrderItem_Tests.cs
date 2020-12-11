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
        private Order _order = new Order();
        public OrderItem_Tests() : base() 
        {
            _order.ServerId = 1;
            DbContext.Orders.Add(_order);
            
            DbContext.SaveChanges();
        }

        [Fact]
        public void Given_Empty_Database_Order_Table_Should_Be_Created()
        {
            Assert.False(DbContext.OrderItems.Any());
        }

        [Fact]
        public void Given_OrderItem_Added_Id_Should_Be_Generated()
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
        public void Given_OrderItem_Added_OrderItem_Should_Be_Persisted()
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
        public void Given_Order_Added_With_OrderItems_Items_Should_Be_Added_To_OrderItem_Table()
        {
            var orderId = _order.Id;
            var expectedCount = _order.Items.Count() + 1;

            var newOrderItem = new OrderItem();
            newOrderItem.OrderId = orderId;
            newOrderItem.MenuItemId = GetLastMenuItemId();

            _order.Items.Add(newOrderItem);
            DbContext.Orders.Attach(_order);
            DbContext.SaveChanges();

            _order = DbContext.Orders.First(o => o.Id == orderId);

            var expectedOrderItemId = GetLastOrderItemId();

            var expectedOrderItem = DbContext.OrderItems.FirstOrDefault(i => i.Id == expectedOrderItemId);

            Assert.NotNull(expectedOrderItem);
            Assert.Equal(expectedCount, _order.Items.Count());

            expectedOrderItem = DbContext.OrderItems.FirstOrDefault(i => i.Id == expectedOrderItemId);

            Assert.NotNull(expectedOrderItem);
        }
    }
}
