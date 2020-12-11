using OrderSystem.Data.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OrderSystem.Messaging.IntegrationTests
{
    public class StationOueueChannelAdapterTests : BaseTest
    {
        private IRabbitMQConnection _connection;
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password",
            };

            _connection = new RabbitMQConnection(factory);
        }

        public override async Task DisposeAsync()
        {
            _connection?.Dispose();

            await base.DisposeAsync();
        }

        [Fact]
        public async void Given_An_Order_With_Multiple_Items_Verify_That_Items_Are_Sent_To_Correct_Queues()
        { 
            StationOueueChannelAdapter adapter = null;
            OrderItem[] order_items = new OrderItem[1];

            MenuItem burger = new MenuItem() { Name = "Burger", Station = Station.Grill };
            MenuItem fries = new MenuItem() { Name = "Fries", Station = Station.Fry };
            MenuItem drink = new MenuItem() { Name = "Fountain Drink", Station = Station.Drink };

            Order order = new Order();
            order.Id = 1;
            order.Items.Add(new OrderItem() { MenuItem = burger, OrderId = 1 });
            order.Items.Add(new OrderItem() { MenuItem = fries, OrderId = 1 });
            order.Items.Add(new OrderItem() { MenuItem = drink, OrderId = 1 });

            order_items = order.Items.ToArray();

           List<Task> tasks = new List<Task>();


            tasks.Add(CreateStationQueueConsumer(Station.Grill, (readGrillOrder) =>
            {
                Assert.Equal(readGrillOrder.OrderId, order_items[0].OrderId);
                Assert.Equal(readGrillOrder.MenuItem.Name, order_items[0].MenuItem.Name);
                Assert.Equal(readGrillOrder.MenuItem.Station, order_items[0].MenuItem.Station);
            }));

            tasks.Add(CreateStationQueueConsumer(Station.Fry, (readFryOrder) =>
            {
                Assert.Equal(readFryOrder.OrderId, order_items[1].OrderId);
                Assert.Equal(readFryOrder.MenuItem.Name, order_items[1].MenuItem.Name);
                Assert.Equal(readFryOrder.MenuItem.Station, order_items[1].MenuItem.Station);
            }));
            tasks.Add(CreateStationQueueConsumer(Station.Drink, (readDrinkOrder) =>
            {
                Assert.Equal(readDrinkOrder.OrderId, order_items[2].OrderId);
                Assert.Equal(readDrinkOrder.MenuItem.Name, order_items[2].MenuItem.Name);
                Assert.Equal(readDrinkOrder.MenuItem.Station, order_items[2].MenuItem.Station);
            }));

            adapter = new StationOueueChannelAdapter(_connection);

            adapter.Send(order);

            await Task.WhenAll(tasks.ToArray());
            
        }

        protected Task CreateStationQueueConsumer(Station station, Action<OrderItem> callback)
        {
            IModel channel = null;

            channel = _connection.CreateChannel();

            channel.ExchangeDeclare(exchange: "orders",
                                type: "direct",
                                durable: true);

            var queueName = channel.QueueDeclare(station.ToString()).QueueName;

            channel.QueueBind(queue: queueName,
                                exchange: "orders",
                                routingKey: station.ToString());

            var consumer = new EventingBasicConsumer(channel);


            OrderItem orderItem = null;

            var tcs = new TaskCompletionSource<bool>();

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                orderItem = JsonSerializer.Deserialize<OrderItem>(json);
                callback(orderItem);
                tcs.SetResult(true);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            return tcs.Task;
        }


    }
}
