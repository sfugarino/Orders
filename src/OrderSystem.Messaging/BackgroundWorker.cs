using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderSystem.Data.Entities;
using OrderSystem.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OrderSystem.Messaging
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly ILogger<BackgroundWorker> _logger;
        private readonly IOrderReceiver _orderReceiver;
        private readonly IOrderForwarder _orderForwarder;
        private readonly IEnumerable<IOrderConsumer> _consumers;

        public BackgroundWorker(IEnumerable<IOrderConsumer> consumers, ILogger<BackgroundWorker> logger, IOrderReceiver orderReceiver, IOrderForwarder orderForwarder)
        {
            _logger = logger;
            _orderReceiver = orderReceiver;
            _orderForwarder = orderForwarder;
            _consumers = consumers;

            _orderReceiver.OnOrderReceived += _orderReceiver_OnOrderReceived;

        }

        private async void _orderReceiver_OnOrderReceived(object sender, OrderReceivedEventArgs e)
        {
            _logger.LogInformation($"Order { e.Order.Id } received");
            await _orderForwarder.ForwardAsync(e.Order);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumerTasks = _consumers.Select(c => c.BeginConsumeAsync(stoppingToken));
            await Task.WhenAll(consumerTasks);
        }
    }
}
