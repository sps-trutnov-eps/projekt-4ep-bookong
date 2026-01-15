using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class ImportRepository(BookongDbContext context) : IImportRepository
    {
        private readonly BookongDbContext _context = context;

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.AsNoTracking().ToListAsync();
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _context.Genres.AsNoTracking().ToListAsync();
        }

        public async Task<List<Kind>> GetAllKindsAsync()
        {
            return await _context.Kinds.AsNoTracking().ToListAsync();
        }

        public async Task<List<Period>> GetAllPeriodsAsync()
        {
            return await _context.Periods.AsNoTracking().ToListAsync();
        }

        public async Task<List<Warehouse>> GetAllWarehousesAsync()
        {
            return await _context.Warehouses.AsNoTracking().ToListAsync();
        }

        public Author? FindAuthor(string fullName, List<Author> authors)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return null;

            var firstName = GetFirstName(fullName);
            var lastName = GetLastName(fullName);

            return authors.FirstOrDefault(a =>
                a.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                a.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));
        }

        public Genre? FindGenre(string name, List<Genre> genres)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return genres.FirstOrDefault(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Kind? FindKind(string name, List<Kind> kinds)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return kinds.FirstOrDefault(k => k.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Period? FindPeriod(string name, List<Period> periods)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return periods.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Warehouse? FindWarehouse(string name, List<Warehouse> warehouses)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return warehouses.FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void CreateAuthorIfNotExists(string fullName, List<Author> authors, ref bool needsCommit)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return;

            var firstName = GetFirstName(fullName);
            var lastName = GetLastName(fullName);
            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var middleName = parts.Length > 2 ? parts[1] : "";

            var existingAuthor = authors.FirstOrDefault(a =>
                a.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                a.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));

            if (existingAuthor == null)
            {
                var author = new Author
                {
                    PublicId = Guid.NewGuid(),
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName
                };
                _context.Authors.Add(author);
                authors.Add(author);
                needsCommit = true;
            }
        }

        public void CreateGenreIfNotExists(string name, List<Genre> genres, ref bool needsCommit)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            var existingGenre = genres.FirstOrDefault(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (existingGenre == null)
            {
                var genre = new Genre
                {
                    PublicId = Guid.NewGuid(),
                    Name = name
                };
                _context.Genres.Add(genre);
                genres.Add(genre);
                needsCommit = true;
            }
        }

        public void CreateKindIfNotExists(string name, List<Kind> kinds, ref bool needsCommit)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            var existingKind = kinds.FirstOrDefault(k => k.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (existingKind == null)
            {
                var kind = new Kind
                {
                    PublicId = Guid.NewGuid(),
                    Name = name
                };
                _context.Kinds.Add(kind);
                kinds.Add(kind);
                needsCommit = true;
            }
        }

        public void CreatePeriodIfNotExists(string name, List<Period> periods, ref bool needsCommit)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            var existingPeriod = periods.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (existingPeriod == null)
            {
                var period = new Period
                {
                    PublicId = Guid.NewGuid(),
                    Name = name
                };
                _context.Periods.Add(period);
                periods.Add(period);
                needsCommit = true;
            }
        }

        public void CreateWarehouseIfNotExists(string name, List<Warehouse> warehouses, ref bool needsCommit)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            var existingWarehouse = warehouses.FirstOrDefault(w => w.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (existingWarehouse == null)
            {
                var defaultAddress = new Address
                {
                    PublicId = Guid.NewGuid(),
                    Street = "N/A",
                    Number = "0",
                    City = "N/A",
                    ZipCode = "00000"
                };

                var warehouse = new Warehouse
                {
                    PublicId = Guid.NewGuid(),
                    Name = name,
                    Address = defaultAddress
                };
                _context.Warehouses.Add(warehouse);
                warehouses.Add(warehouse);
                needsCommit = true;
            }
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
        }

        private static string GetFirstName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "";

            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts[0] : "";
        }

        private static string GetLastName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return "";

            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 1 ? parts[^1] : "";
        }
    }
}
