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
    class StationChannelAdapter
    {
        readonly IConnection _connection = null;
        readonly IModel _channel = null;

        private bool isDisposed;
        public StationChannelAdapter(string hostName)
        {
            var factory = new ConnectionFactory() { HostName = "hostName" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "order_items",
                                    type: "direct");
        }

        ~StationChannelAdapter()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        public void Send(Order order)
        {
            foreach (var item in order.Items)
            {
                var body = JsonSerializer.Serialize(item);
                byte[] bytes = Encoding.ASCII.GetBytes(body);


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
                if (_channel != null)
                {
                    _channel.Dispose();
                }

                if (_connection != null)
                {
                    _connection.Dispose();
                }
            }

            isDisposed = true;
        }
    }

}

