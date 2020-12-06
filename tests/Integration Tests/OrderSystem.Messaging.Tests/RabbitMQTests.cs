using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrderSystem.Messaging.Tests
{
    public class RabbitMQTests : BaseTest
    {
        [Fact]
        public void Test1()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password",
                RequestedConnectionTimeout = new TimeSpan(0, 1, 30)
            };
            using (var connection = factory.CreateConnection())
            {
                Assert.NotNull(connection);
            }
            
        }
    }
}
