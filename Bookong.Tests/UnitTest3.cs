//using Bookong.Infrastructure.Persistence;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Xunit;

//namespace Bookong.Tests
//{
//    public class UnitTest2
//    {
//        [Fact]
//        public void Test_DatabaseConnection()
//        {
//            // Arrange: Nastavení DbContext s databází v paměti
//            var serviceProvider = new ServiceCollection()
//                .AddDbContext<ApplicationDbContext>(options =>
//                    options.UseInMemoryDatabase("TestDatabase"))
//                .BuildServiceProvider();

//            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

//            // Act: Ověření připojení k databázi
//            var canConnect = dbContext.Database.CanConnect();

//            // Assert: DbContext by měl být schopen se připojit
//            Assert.True(canConnect);
//        }

//        [Fact]
//        public void Test_EntityIsSavedToDatabase()
//        {
//            // Arrange: Nastavení DbContext s databází v paměti
//            var serviceProvider = new ServiceCollection()
//                .AddDbContext<ApplicationDbContext>(options =>
//                    options.UseInMemoryDatabase("TestDatabase"))
//                .BuildServiceProvider();

//            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

//            // Přidání testovací entity
//            var testEntity = new TestEntity { Id = 1, Name = "Test Entity" };
//            dbContext.Add(testEntity);

//            // Act: Uložení změn do databáze
//            dbContext.SaveChanges();

//            // Assert: Ověření, že entita byla uložena
//            var savedEntity = dbContext.Set<TestEntity>().Find(1);
//            Assert.NotNull(savedEntity);
//            Assert.Equal("Test Entity", savedEntity.Name);
//        }
//    }

//    // Testovací entita
//    public class TestEntity
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }
//    }
//}
