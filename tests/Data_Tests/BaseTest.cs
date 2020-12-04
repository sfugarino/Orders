using System;
using data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Data_Tests
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
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
