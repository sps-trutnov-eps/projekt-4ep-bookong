using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class WarehouseRepository(BookongDbContext context) : IWarehouseRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
        }

        public void Delete(Warehouse warehouse)
        {
            _context.Warehouses.Remove(warehouse);
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Include(w => w.Address)
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Include(w => w.Address)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Warehouse?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.Warehouses
                .AsNoTracking()
                .Include(w => w.Address)
                .FirstOrDefaultAsync(w => w.PublicId == publicId);
        }

        public async Task<PagedResult<Warehouse>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Warehouses
                .AsNoTracking()
                .Include(w => w.Address)
                .OrderBy(w => w.Name);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Warehouse>(items, totalCount, pageNumber, pageSize);
        }

        public void Update(Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
        }
    }
}
