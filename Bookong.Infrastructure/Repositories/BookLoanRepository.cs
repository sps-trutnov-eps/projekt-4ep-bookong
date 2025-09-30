using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class BookLoanRepository(BookongDbContext context) : IBookLoanRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(BookLoan bookLoan)
        {
            throw new NotImplementedException();
        }

        public void Delete(BookLoan bookLoan)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookLoan>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BookLoan?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BookLoan?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<BookLoan>> GetByUserAsync(User user, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<BookLoan>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(BookLoan bookLoan)
        {
            throw new NotImplementedException();
        }
    }
}
