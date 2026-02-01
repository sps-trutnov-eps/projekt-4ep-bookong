using System;
using System.Threading.Tasks;
using Bookong.Domain.Entities;
using Bookong.Infrastructure.Data;
using Bookong.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Bookong.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public async Task GenreRepo_AddGet()
        {
            var options = new DbContextOptionsBuilder<BookongDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new BookongDbContext(options);
            var repository = new GenreRepository(context);

            var genre = new Genre { Name = "Test Genre" };
            repository.Add(genre);
            await context.SaveChangesAsync();

            var genres = await repository.GetAllAsync();
            Assert.Contains(genres, g => g.Name == "Test Genre");
        }

        [Fact]
        public async Task AuthorRepo_AddGet()
        {
            var options = new DbContextOptionsBuilder<BookongDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new BookongDbContext(options);
            var repository = new AuthorRepository(context);
            var author = new Author
            {
                FirstName = "Test",
                LastName = "Author",
                MiddleName = "Middle",
                PublicId = Guid.NewGuid()
            };
            repository.Add(author);
            await context.SaveChangesAsync();

            var authors = await repository.GetAllAsync();
            Assert.Contains(authors, a => a.FirstName == "Test" && a.LastName == "Author");
        }

        [Fact]
        public async Task BookRepo_AddGet()
        {
            var options = new DbContextOptionsBuilder<BookongDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new BookongDbContext(options);
            var repository = new BookRepository(context);
            var book = new Book { Name = "Test Book", Pages = 100 };
            repository.Add(book);
            await context.SaveChangesAsync();

            var books = await repository.GetAllAsync();
            Assert.Contains(books, b => b.Name == "Test Book" && b.Pages == 100);
        }

        [Fact]
        public async Task KindRepo_AddGet()
        {
            var options = new DbContextOptionsBuilder<BookongDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new BookongDbContext(options);
            var repository = new KindRepository(context);
            var kind = new Kind { Name = "Test Kind" };
            repository.Add(kind);
            await context.SaveChangesAsync();

            var kinds = await repository.GetAllAsync();
            Assert.Contains(kinds, k => k.Name == "Test Kind");
        }

        [Fact]
        public async Task PeriodRepo_AddGet()
        {
            var options = new DbContextOptionsBuilder<BookongDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new BookongDbContext(options);
            var repository = new PeriodRepository(context);
            var period = new Period { Name = "Test Period" };
            repository.Add(period);
            await context.SaveChangesAsync();

            var periods = await repository.GetAllAsync();
            Assert.Contains(periods, p => p.Name == "Test Period");
        }

        [Fact]
        public async Task PublisherRepo_AddGet()
        {
            var options = new DbContextOptionsBuilder<BookongDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new BookongDbContext(options);
            var repository = new PublisherRepository(context);
            var publisher = new Publisher
            {
                Name = "Test Publisher",
                PublicId = Guid.NewGuid()
            };
            repository.Add(publisher);
            await context.SaveChangesAsync();

            var publishers = await repository.GetAllAsync();
            Assert.Contains(publishers, p => p.Name == "Test Publisher");
        }

        [Fact]
        public async Task WarehouseRepo_AddGet()
        {
            var options = new DbContextOptionsBuilder<BookongDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new BookongDbContext(options);
            var repository = new WarehouseRepository(context);
            var warehouse = new Warehouse
            {
                Name = "Test Warehouse",
                Address = new Address
                {
                    Street = "Test St",
                    City = "Test City",
                    Number = "1",
                    ZipCode = "12345"
                }
            };
            repository.Add(warehouse);
            await context.SaveChangesAsync();

            var warehouses = await repository.GetAllAsync();
            Assert.Contains(warehouses, w => w.Name == "Test Warehouse");
        }
    }
}
