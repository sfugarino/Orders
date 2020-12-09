using OrderSystem.Data.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderSystem.Messaging
{
    public class OrderReceiver: IOrderReceiver
    {
        private bool isDisposed;
        private IConnection _connection = null;
        private IModel _channel = null;

        public event EventHandler<OrderReceivedEventArgs> OnOrderReceived;

        public OrderReceiver(string hostname)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnection();
        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var order = JsonSerializer.Deserialize<Order>(message);
            Console.WriteLine(" [x] Received Order {0}", order.Id);

            return Task.Run(() =>
            {
                if (OnOrderReceived != null)
                {
                    OnOrderReceived(this, new OrderReceivedEventArgs { Order = order });
                }
            });
        }

        private void InitChannel()
        {
            _channel?.Dispose();

            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "order_queue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            _channel.CallbackException += (sender, ea) =>
            {
                InitChannel();
                InitSubscription();
            };
        }

        private void InitSubscription()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;
        }

        public void Start()
        {
            InitChannel();
            InitSubscription();

        }

        ~OrderReceiver()
        {
            Dispose(false);
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
                _connection?.Dispose();
            }

            isDisposed = true;
        }
    }
}
