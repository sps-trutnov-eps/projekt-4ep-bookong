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
                .AsNoTracking()
                .Include(bl => bl.Book)
                    .ThenInclude(b => b.Author)
                .Include(bl => bl.Book)
                    .ThenInclude(b => b.Period)
                .Include(bl => bl.Book)
                    .ThenInclude(b => b.Kind)
                .Include(bl => bl.User)
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

        public async Task<PagedResult<BookLoan>> GetByUserAsync(User user, int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.BookLoans
                .AsNoTracking()
                .Include(bl => bl.Book)
                    .ThenInclude(b => b.Author)
                .Include(bl => bl.Book)
                    .ThenInclude(b => b.Period)
                .Include(bl => bl.Book)
                    .ThenInclude(b => b.Kind)
                .Include(bl => bl.User)
                .Where(bl => bl.UserId == user.Id);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(bl => bl.LoanDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<BookLoan>(items, totalCount, pageNumber, pageSize);
        }

        public async Task<PagedResult<BookLoan>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.BookLoans
                .AsNoTracking()
                .Include(bl => bl.Book)
                    .ThenInclude(b => b.Author)
                .Include(bl => bl.Book)
                    .ThenInclude(b => b.Period)
                .Include(bl => bl.Book)
                    .ThenInclude(b => b.Kind)
                .Include(bl => bl.User);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(bl => bl.LoanDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<BookLoan>(items, totalCount, pageNumber, pageSize);
        }

        public void Update(BookLoan bookLoan)
        {
            _context.BookLoans.Update(bookLoan);
        }
    }
}
