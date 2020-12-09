using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderSystem.Data;
using OrderSystem.Data.Entities;
using OrderSystem.Messaging;

namespace OrderSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IRepository<Order> _repository;
        private readonly IChannelAdapter _channelAddpter;

        public OrdersController(ILogger<OrdersController> logger, IRepository<Order> repository, IChannelAdapter channelAddpter)
        {
            _logger = logger;
            _repository = repository;
            _channelAddpter = channelAddpter;
        }

        [HttpGet]
        public async Task<ActionResult<Order>> Get()
        {
            try
            {
                var orders = await _repository.GetAllAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message);

            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(Order order)
        {
            try
            {
                var entityEntry = await _repository.InsertAsync(order);
                await _repository.SaveAsync();
                _channelAddpter.Send(order);

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(Order order)
        {
            try
            {
                _repository.Update(order);
                await _repository.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int order)
        {
            try
            {
                _repository.Delete(order);
                await _repository.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message);
            }
        }
    }
}
