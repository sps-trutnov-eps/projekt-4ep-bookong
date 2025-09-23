using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Data
{
    public class BookongDbContext : DbContext
    {
        public BookongDbContext(DbContextOptions<BookongDbContext> options)
            : base(options) { }

        public DbSet<Bookong.Domain.Entities.User> Users { get; set; }
        public DbSet<Bookong.Domain.Entities.Book> Books { get; set; }
        public DbSet<Bookong.Domain.Entities.Author> Authors { get; set; }
        public DbSet<Bookong.Domain.Entities.Genre> Genres { get; set; }
        public DbSet<Bookong.Domain.Entities.Kind> Kinds { get; set; }
        public DbSet<Bookong.Domain.Entities.Period> Periods { get; set; }
        public DbSet<Bookong.Domain.Entities.BookLoan> BookLoans { get; set; }
        public DbSet<Bookong.Domain.Entities.Warehouse> Warehouses { get; set; }
        public DbSet<Bookong.Domain.Entities.Address> Addresses { get; set; }
    }
}
