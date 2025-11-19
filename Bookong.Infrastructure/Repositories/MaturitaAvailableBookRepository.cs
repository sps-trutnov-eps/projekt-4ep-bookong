using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class MaturitaAvailableBookRepository : IMaturitaAvailableBookRepository
    {
        private readonly BookongDbContext _context;

        public MaturitaAvailableBookRepository(BookongDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaturitaAvailableBook>> GetAllAsync()
        {
            return await _context.MaturitaAvailableBooks
                .Include(x => x.Book)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<MaturitaAvailableBook?> GetByIdAsync(int id)
        {
            return await _context.MaturitaAvailableBooks.FindAsync(id);
        }

        public async Task<IEnumerable<MaturitaAvailableBook>> GetByBookIdAsync(int bookId)
        {
            return await _context.MaturitaAvailableBooks
                .Where(x => x.BookId == bookId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<MaturitaAvailableBook>> GetByUserIdAsync(int userId)
        {
            return await _context.MaturitaAvailableBooks
                .Where(x => x.CreatedByUserId == userId)
                .Include(x => x.Book)
                .AsNoTracking()
                .ToListAsync();
        }

        public void Add(MaturitaAvailableBook entity)
        {
            _context.MaturitaAvailableBooks.Add(entity);
        }

        public void Delete(MaturitaAvailableBook entity)
        {
            _context.MaturitaAvailableBooks.Remove(entity);
        }
    }
}
