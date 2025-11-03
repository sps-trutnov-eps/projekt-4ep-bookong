using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Domain.Queries;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class BookRepository(BookongDbContext context) : IBookRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Book book)
        {
            _context.Books.Add(book);
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Kind)
                .Include(b => b.Period)
                .Include(b => b.Publisher)
                .Include(b => b.Warehouse)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Kind)
                .Include(b => b.Period)
                .Include(b => b.Publisher)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Kind)
                .Include(b => b.Period)
                .Include(b => b.Publisher)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(b => b.PublicId == publicId);
        }

        public async Task<PagedResult<Book>> GetFilteredAsync(BookSearchCriteria criteria, int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Kind)
                .Include(b => b.Period)
                .Include(b => b.Publisher)
                .Include(b => b.Warehouse)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(criteria.Title))
            {
                var title = criteria.Title.Trim();
                query = query.Where(b => b.Name.Contains(title));
            }

            if (criteria.AuthorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == criteria.AuthorId.Value);
            }

            if (criteria.GenreId.HasValue)
            {
                query = query.Where(b => b.GenreId == criteria.GenreId.Value);
            }

            if (criteria.KindId.HasValue)
            {
                query = query.Where(b => b.KindId == criteria.KindId.Value);
            }

            if (criteria.PeriodId.HasValue)
            {
                query = query.Where(b => b.PeriodId == criteria.PeriodId.Value);
            }

            if (!string.IsNullOrWhiteSpace(criteria.Publisher))
            {
                var pub = criteria.Publisher.Trim();
                query = query.Where(b => b.Publisher != null && b.Publisher.Name.Contains(pub));
            }

            // Sorting
            query = criteria.SortBy?.ToLowerInvariant() switch
            {
                "title" => query.OrderBy(b => b.Name),
                "author" => query.OrderBy(b => b.Author.LastName).ThenBy(b => b.Author.FirstName),
                "genre" => query.OrderBy(b => b.Genre.Name),
                "kind" => query.OrderBy(b => b.Kind.Name),
                "period" => query.OrderBy(b => b.Period.Name),
                "publisher" => query.OrderBy(b => b.Publisher != null).ThenBy(b => b.Publisher!.Name),
                "pages" => query.OrderBy(b => b.Pages),
                _ => query.OrderBy(b => b.Name)
            };

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Book>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<PagedResult<Book>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Kind)
                .Include(b => b.Period)
                .Include(b => b.Publisher)
                .Include(b => b.Warehouse)
                .OrderBy(b => b.Name);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Book>(items, totalCount, pageNumber, pageSize);
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Books.CountAsync();
        }

        public async Task<IEnumerable<Bookong.Domain.Entities.Book>> GetForExportAsync(int[] ids)
        {
            var query = _context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Include(b => b.Kind)
            .Include(b => b.Period)
            .Include(b => b.Publisher)
            .Include(b => b.Warehouse)
            .AsQueryable();

            if (ids != null && ids.Length > 0)
            {
                query = query.Where(b => ids.Contains(b.Id));
            }

            return await query.ToListAsync();
        }

        public async Task<int[]> GetAllIDsAsync()
        {
            return await _context.Books
                .Select(b => b.Id)
                .ToArrayAsync();
        }

    }
}
