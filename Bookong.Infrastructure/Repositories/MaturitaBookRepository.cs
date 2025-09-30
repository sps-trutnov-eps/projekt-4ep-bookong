using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Domain.Queries;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class MaturitaBookRepository(BookongDbContext context) : IMaturitaBookRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(MaturitaBook maturitaBook)
        {
            throw new NotImplementedException();
        }

        public void Delete(MaturitaBook maturitaBook)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<MaturitaBook>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MaturitaBook?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MaturitaBook?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<MaturitaBook>> GetFilteredAsync(BookSearchCriteria criteria, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(MaturitaBook maturitaBook)
        {
            throw new NotImplementedException();
        }
    }
}
