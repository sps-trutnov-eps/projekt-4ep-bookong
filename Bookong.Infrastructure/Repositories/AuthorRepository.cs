using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class AuthorRepository(BookongDbContext context) : IAuthorRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Author author)
        {
            _context.Authors.Add(author);
        }

        public void Delete(Author author)
        {
            _context.Authors.Remove(author);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors
                .AsNoTracking()
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PagedResult<Author>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Authors
                .AsNoTracking()
                .Where(a => a.FirstName.Contains(name) || (a.MiddleName != null && a.MiddleName.Contains(name)) || a.LastName.Contains(name))
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Author>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<PagedResult<Author>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Authors
                .AsNoTracking()
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Author>(items, totalCount, pageNumber, pageSize);
        }

        public void Update(Author author)
        {
            _context.Authors.Update(author);
        }
    }
}
