using Microsoft.Extensions.Logging;
using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OrderSystem.Messaging
{
    public class OrderForwarder
    {
        private readonly ChannelWriter<OrderItem> _writer;
        private readonly ILogger<OrderForwarder> _logger;

        public OrderForwarder(ChannelWriter<OrderItem> writer, ILogger<OrderForwarder> logger)
        {
            _writer = writer;
            _logger = logger;
        }

        public async Task ForwardAsync(Order order, CancellationToken cancellationToken = default)
        {
            foreach (var item in order.Items)
            { 
                await _writer.WriteAsync(item, cancellationToken);
                _logger.LogInformation($"Producer > published message {item.Id} '{item.Station}'");
            }
        }
    }
}
