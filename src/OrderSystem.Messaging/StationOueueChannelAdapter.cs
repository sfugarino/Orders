using OrderSystem.Data;
using OrderSystem.Data.Entities;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderSystem.Messaging
{
    public class StationOueueChannelAdapter : IStationQueueChannelAdapter
    {
        readonly IRabbitMQConnection _connection = null;
        readonly IModel _channel = null;

        private bool isDisposed;
        public StationOueueChannelAdapter(IRabbitMQConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateChannel();
            _channel.ExchangeDeclare(exchange: "order_items",
                                    type: "direct");
        }

        ~StationOueueChannelAdapter()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        public void Send(Order order)
        {
            foreach (var item in order.Items)
            {
                var body = JsonSerializer.Serialize(item);
                byte[] bytes = Encoding.UTF8.GetBytes(body);


                _channel.BasicPublish(exchange: "orders_items",
                                     routingKey: item.Station.ToString(),
                                     true,
                                     basicProperties: null,
                                     body: bytes.AsMemory());
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                _channel?.Dispose();
            }

            isDisposed = true;
        }
    }

}

