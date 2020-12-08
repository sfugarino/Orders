using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderSystem.Data;
using OrderSystem.Data.Entities;

namespace OrderSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IRepository<Order> _repository;
        public OrdersController(ILogger<OrdersController> logger, IRepository<Order> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<Order>> Get()
        {
            var orders = await _repository.GetAllAsync();
            return Ok(orders);
        }
    }
}
