using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Messaging
{
    public interface IRabbitMQConnection : IDisposable
    {
        IConnection Connection { get; }
        bool IsConnected { get; }

        IModel CreateChannel();
    }
}
