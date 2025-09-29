using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class PeriodRepository(BookongDbContext context) : IPeriodRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Period period)
        {
            throw new NotImplementedException();
        }

        public void Delete(Period period)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Period>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Period?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Period>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<Period?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public void Update(Period period)
        {
            throw new NotImplementedException();
        }
    }
}
