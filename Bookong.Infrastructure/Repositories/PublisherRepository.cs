using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class PublisherRepository(BookongDbContext context) : IPublisherRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Publisher publisher)
        {
            throw new NotImplementedException();
        }

        public void Delete(Publisher publisher)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Publisher>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Publisher?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Publisher>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<Publisher?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Publisher>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(Publisher publisher)
        {
            throw new NotImplementedException();
        }
    }
}
