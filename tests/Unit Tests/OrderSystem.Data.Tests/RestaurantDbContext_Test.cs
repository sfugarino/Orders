using System.Threading.Tasks;
using Xunit;

namespace OrderSystem.Data.Tests
{
    public class RestaurantDbContext_Test : BaseTest
    {
        [Fact]
        public async Task DatabaseIsAvailableAndCanBeConnectedTo()
        {
            Assert.True(await DbContext.Database.CanConnectAsync());
        }
    }
}
