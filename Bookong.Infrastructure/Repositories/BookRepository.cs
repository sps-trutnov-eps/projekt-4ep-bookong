using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Domain.Queries;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class BookRepository(BookongDbContext context) : IBookRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Book book)
        {
            throw new NotImplementedException();
        }

        public void Delete(Book book)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Book?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Book?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Book>> GetFilteredAsync(BookSearchCriteria criteria, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Book>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
