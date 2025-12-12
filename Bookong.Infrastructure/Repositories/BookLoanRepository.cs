using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class BookLoanRepository(BookongDbContext context) : IBookLoanRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(BookLoan bookLoan)
        {
            _context.BookLoans.Add(bookLoan);
        }

        public void Delete(BookLoan bookLoan)
        {
            _context.BookLoans.Remove(bookLoan);
        }

        public async Task<IEnumerable<BookLoan>> GetAllAsync()
        {
            return await _context.BookLoans
                .Include(bl => bl.Book)
                .ThenInclude(b => b.Author)
                .Include(bl => bl.Book)
                .ThenInclude(b => b.Genre)
                .ToListAsync();
        }

        public async Task<BookLoan?> GetByIdAsync(int id)
        {
            return await _context.BookLoans.FindAsync(id).AsTask();
        }

        public async Task<BookLoan?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.BookLoans.FirstOrDefaultAsync(bl => bl.PublicId == publicId);
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
            _context.BookLoans.Update(bookLoan);
        }
    }
}
