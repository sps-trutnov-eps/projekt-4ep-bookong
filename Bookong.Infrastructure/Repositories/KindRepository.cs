using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class KindRepository(BookongDbContext context) : IKindRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Kind kind)
        {
            _context.Kinds.Add(kind);
        }

        public void Delete(Kind kind)
        {
            _context.Kinds.Remove(kind);
        }

        public async Task<IEnumerable<Kind>> GetAllAsync()
        {
            return await _context.Kinds
                .AsNoTracking()
                .OrderBy(k => k.Name)
                .ToListAsync();
        }

        public async Task<Kind?> GetByIdAsync(int id)
        {
            return await _context.Kinds
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<Kind?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.Kinds
                .AsNoTracking()
                .FirstOrDefaultAsync(k => k.PublicId == publicId);
        }

        public async Task<PagedResult<Kind>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Kinds
                .AsNoTracking()
                .Where(k => k.Name.Contains(name))
                .OrderBy(k => k.Name);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Kind>(items, totalCount, pageNumber, pageSize);
        }

        public void Update(Kind kind)
        {
            _context.Kinds.Update(kind);
        }
    }
}
