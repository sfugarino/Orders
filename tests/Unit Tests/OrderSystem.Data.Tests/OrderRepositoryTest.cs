using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OrderSystem.Data.Entities;

namespace OrderSystem.Data.Tests
{
    public class OrderRepositoryTest : BaseTest
    {
        [Fact]
        public void GivenOrderInsertIntoDBUsingRepository()
        {
            Repository<Order> repository = new Repository<Order>(DbContext);
            var newOrder = new Order();
            newOrder.ServerId = 1;

        }
    }
}
