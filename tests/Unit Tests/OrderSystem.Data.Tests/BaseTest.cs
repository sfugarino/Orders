using System;
using OrderSystem.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OrderSystem.Data.Entities;
using System.Linq;

namespace OrderSystem.Data.Tests
{
    public class BaseTest : IDisposable
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        protected readonly RestaurantDbContext DbContext;
        protected BaseTest()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.Open();
            var options = new DbContextOptionsBuilder<RestaurantDbContext>()
                    .UseSqlite(_connection)
                    .Options;
            DbContext = new RestaurantDbContext(options);
        }

        public void Dispose()
        {
            _connection.Close();
        }

        protected int GetLastOrderId()
        {
            int? last = DbContext.Orders.OrderBy(o => o.Id).Select(o => o.Id).LastOrDefault();
            return last.HasValue ? last.Value : 0;
        }

        protected int GetLastOrderItemId()
        {
            int? last = DbContext.OrderItems.OrderBy(o => o.Id).Select(o => o.Id).LastOrDefault();
            return last.HasValue ? last.Value : 0;
        }


        protected int GetLastMenuItemId()
        {
            int? last = DbContext.MenuItems.OrderBy(o => o.Id).Select(o => o.Id).LastOrDefault();
            return last.HasValue ? last.Value : 0;
        }
    }
}
