using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class PeriodRepository(BookongDbContext context) : IPeriodRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Period period)
        {
            _context.Periods.Add(period);
        }

        public void Delete(Period period)
        {
            _context.Periods.Remove(period);
        }

        public async Task<IEnumerable<Period>> GetAllAsync()
        {
            return await _context.Periods
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Period?> GetByIdAsync(int id)
        {
            return await _context.Periods
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Period?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.Periods
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PublicId == publicId);
        }

        public async Task<PagedResult<Period>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Periods
                .AsNoTracking()
                .Where(p => p.Name.Contains(name))
                .OrderBy(p => p.Name);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Period>(items, totalCount, pageNumber, pageSize);
        }

        public void Update(Period period)
        {
            _context.Periods.Update(period);
        }
    }
}
