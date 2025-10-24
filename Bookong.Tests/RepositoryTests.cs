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
 public async Task GenreRepository_AddAndGetById_WorksWithInMemoryDb()
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
 }
}
