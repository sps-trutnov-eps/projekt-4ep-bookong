using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookongDbContext>
    {
        public BookongDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine("..", "Bookong.Web", "appsettings.json"))
                .Build();

            var builder = new DbContextOptionsBuilder<BookongDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);

            return new BookongDbContext(builder.Options);
        }
    }
}