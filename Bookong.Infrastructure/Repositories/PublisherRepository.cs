using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class PublisherRepository(BookongDbContext context) : IPublisherRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
        }

        public void Delete(Publisher publisher)
        {
            _context.Publishers.Remove(publisher);
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            return await _context.Publishers
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Publisher?> GetByIdAsync(int id)
        {
            return await _context.Publishers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Publisher?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.Publishers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PublicId == publicId);
        }

        public async Task<PagedResult<Publisher>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Publishers
                .AsNoTracking()
                .Where(p => p.Name.Contains(name))
                .OrderBy(p => p.Name);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Publisher>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<PagedResult<Publisher>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Publishers
                .AsNoTracking()
                .OrderBy(p => p.Name);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Publisher>(items, totalCount, pageNumber, pageSize);
        }

        public void Update(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
        }
    }
}
