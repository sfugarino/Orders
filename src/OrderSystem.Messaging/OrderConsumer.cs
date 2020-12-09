using Microsoft.Extensions.Logging;
using OrderSystem.Data;
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
    public class OrderConsumer : IOrderConsumer
    {
        private readonly ILogger<OrderConsumer> _logger;
        private readonly int _instanceId;
        private readonly ChannelReader<Order> _reader;
        private IRepository<Order> _repository;

        public OrderConsumer(int instanceId, ILogger<OrderConsumer> logger, ChannelReader<Order> reader, IRepository<Order> repository)
        {
            _instanceId = instanceId;
            _logger = logger;
            _reader = reader;
            _repository = repository;
        }

        public async Task BeginConsumeAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Consumer {_instanceId} > starting");

            try
            {
                await foreach (var message in _reader.ReadAllAsync(cancellationToken))
                {
                    _logger.LogInformation($"CONSUMER ({_instanceId})> Received order {message.Id} ");
                    await Task.Delay(500, cancellationToken);
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning($"Consumer {_instanceId} > forced stop");
            }

            _logger.LogInformation($"Consumer {_instanceId} > shutting down");
        }
    }
}
