using Bookong.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Data
{
    public class BookongDbContext : DbContext
    {
        public BookongDbContext(DbContextOptions<BookongDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Kind> Kinds { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<MaturitaBook> MaturitaBooks { get; set; }
        public DbSet<MaturitaBookSelection> MaturitaBookSelections { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<MaturitaBookAssignmentRequest> MaturitaBookAssignmentRequests { get; set; }
        public DbSet<BookReservation> BookReservations { get; set; }
        public DbSet<TeachingMaterial> TeachingMaterials { get; set; }
    }
}
