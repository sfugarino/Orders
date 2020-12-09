using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Data.Entities;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderSystem.Messaging
{
    public class OrderQueueChannelAdapter : IChannelAdapter
    {
        readonly IConnection _connection = null;
        readonly IModel _channel = null;

        private bool isDisposed;
        public OrderQueueChannelAdapter(string hostname)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "order_queue",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
        }

        ~OrderQueueChannelAdapter()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        public void Send(Order order)
        {

            var body = JsonSerializer.Serialize(order);
            byte[] bytes = Encoding.UTF8.GetBytes(body);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: "",
                                    routingKey: "order_queue",
                                    true,
                                    basicProperties: properties,
                                    body: bytes.AsMemory());
 
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
