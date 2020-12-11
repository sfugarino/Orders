using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OrderSystem.Data.Entities;
using RabbitMQ.Client;
using Moq;
using System.Text.Json;

namespace OrderSystem.Messaging.UnitTests
{
    public class StationOueueChannelAdapterTests
    {
        private readonly IConnectionFactory _connectionFaction = new ConnectionFactory();
        private readonly IRabbitMQConnection _rabbitMQConnection;
        private Mock<IModel> _stationQueueChannelMoc;
        private Queue<ReadOnlyMemory<byte>> result_queue;

        public StationOueueChannelAdapterTests()
        {
            IDictionary<string, object> args = new Dictionary<string, object>();

            _stationQueueChannelMoc = new Mock<IModel>();

            _stationQueueChannelMoc.Setup(model => model.ExchangeDeclare(It.IsAny<string>(), It.IsAny<string>(),false, false, null))
                .Callback(() => result_queue = new Queue<ReadOnlyMemory<byte>>())
                .Verifiable();

            _stationQueueChannelMoc.Setup(model => model.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), true, null, It.IsAny<ReadOnlyMemory<byte>>()))
                .Callback<string, string, bool, IBasicProperties, ReadOnlyMemory<byte>>((exchange, routingKey, mandatory, basicProperties, body) =>
                {
                    result_queue.Enqueue(body);
                })
                .Verifiable();

            var rabbitMQConnectionMoq = new Mock<IRabbitMQConnection>();

            var stationQueueChannel = _stationQueueChannelMoc.Object;

            rabbitMQConnectionMoq.Setup(connection => connection.CreateChannel()).Returns(stationQueueChannel);

            _rabbitMQConnection = rabbitMQConnectionMoq.Object;

        }

        [Fact]
        public void Given_An_Order_With_Multiple_Items_Items_Are_Serialized_And_Sent_To_Exchange()
        {
            MenuItem burger = new MenuItem() { Name = "Burger", Station = Station.Grill };
            MenuItem fries = new MenuItem() { Name = "Fries", Station = Station.Fry };
            MenuItem drink = new MenuItem() { Name = "Fountain Drink", Station = Station.Drink };

            Order order = new Order();
            order.Id = 1;
            order.Items.Add(new OrderItem() { MenuItem = burger, OrderId = 1 });
            order.Items.Add(new OrderItem() { MenuItem = fries, OrderId = 1 });
            order.Items.Add(new OrderItem() { MenuItem = drink, OrderId = 1 });

            StationOueueChannelAdapter adapter = new StationOueueChannelAdapter(_rabbitMQConnection);

            adapter.Send(order);

            _stationQueueChannelMoc.Verify(mock => mock.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), true, null, It.IsAny<ReadOnlyMemory<byte>>()), Times.Exactly(3));

            var burger_memory = result_queue.Dequeue();
            
            var burger_bytes = burger_memory.ToArray();
            var burger_json = Encoding.UTF8.GetString(burger_bytes);
            var processed_burger = JsonSerializer.Deserialize<OrderItem>(burger_json);

            var fries_memory = result_queue.Dequeue();

            var fries_bytes = fries_memory.ToArray();
            var fries_json = Encoding.UTF8.GetString(fries_bytes);
            var processed_fries = JsonSerializer.Deserialize<OrderItem>(fries_json);

            var drink_memory = result_queue.Dequeue();

            var drink_bytes = drink_memory.ToArray();
            var drink_json = Encoding.UTF8.GetString(drink_bytes);
            var processed_drink = JsonSerializer.Deserialize<OrderItem>(drink_json);

            var order_items = order.Items.ToArray();

            Assert.Equal(processed_burger.OrderId, order_items[0].OrderId);
            Assert.Equal(processed_burger.MenuItem.Name, order_items[0].MenuItem.Name);
            Assert.Equal(processed_burger.MenuItem.Station, order_items[0].MenuItem.Station);

            Assert.Equal(processed_fries.OrderId, order_items[1].OrderId);
            Assert.Equal(processed_fries.MenuItem.Name, order_items[1].MenuItem.Name);
            Assert.Equal(processed_fries.MenuItem.Station, order_items[1].MenuItem.Station);

            Assert.Equal(processed_drink.OrderId, order_items[2].OrderId);
            Assert.Equal(processed_drink.MenuItem.Name, order_items[2].MenuItem.Name);
            Assert.Equal(processed_drink.MenuItem.Station, order_items[2].MenuItem.Station);

            Assert.NotNull(adapter);
        }
    }
}
