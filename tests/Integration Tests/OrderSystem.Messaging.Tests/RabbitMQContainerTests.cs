using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrderSystem.Messaging.IntegrationTests
{
    public class RabbitMQContainerTests : BaseTest
    {
        [Fact]
        public void Test1()
        {
           
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password",
            };

            using (var connection = factory.CreateConnection())
            {
                Assert.NotNull(connection);
            }
            
        }
    }
}
