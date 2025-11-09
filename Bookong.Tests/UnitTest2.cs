using Bookong.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bookong.Tests
{
    public class UnitTest2
    {
        [Fact]
        public void Test_DatabaseConnection()
        {
            // Arrange: Nastavení DbContext s databází v paměti
            var serviceProvider = new ServiceCollection()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase"))
                .BuildServiceProvider();

            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Act: Ověření připojení k databázi
            var canConnect = dbContext.Database.CanConnect();

            // Assert: DbContext by měl být schopen se připojit
            Assert.True(canConnect);
        }
    }
}
