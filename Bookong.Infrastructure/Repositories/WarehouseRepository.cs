using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class WarehouseRepository(BookongDbContext context) : IWarehouseRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Warehouse warehouse)
        {
            throw new NotImplementedException();
        }

        public void Delete(Warehouse warehouse)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Warehouse?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Warehouse?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Warehouse>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(Warehouse warehouse)
        {
            throw new NotImplementedException();
        }
    }
}
