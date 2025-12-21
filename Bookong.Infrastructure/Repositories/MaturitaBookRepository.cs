using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Domain.Queries;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class MaturitaBookRepository(BookongDbContext context) : IMaturitaBookRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(MaturitaBook maturitaBook)
        {
            _context.MaturitaBooks.Add(maturitaBook);
        }

        public void Delete(MaturitaBook maturitaBook)
        {
            _context.MaturitaBooks.Remove(maturitaBook);
        }

        public Task<PagedResult<MaturitaBook>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MaturitaBook>> GetAllWithDetailsAsync()
        {
            return await _context.MaturitaBooks
                .AsNoTracking()
                .Include(mb => mb.Author)
                .Include(mb => mb.Genre)
                .Include(mb => mb.Kind)
                .Include(mb => mb.Period)
                .ToListAsync();
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

        public async Task<MaturitaBook?> FindExistingAsync(string name, int authorId, int genreId, int kindId, int periodId)
        {
            return await _context.MaturitaBooks
                .Include(mb => mb.Author)
                .Include(mb => mb.Genre)
                .Include(mb => mb.Kind)
                .Include(mb => mb.Period)
                .Where(mb => mb.Name == name
                          && mb.Author!.Id == authorId
                          && mb.Genre!.Id == genreId
                          && mb.Kind!.Id == kindId
                          && mb.Period!.Id == periodId)
                .FirstOrDefaultAsync();
        }

        public void Update(MaturitaBook maturitaBook)
        {
            _context.MaturitaBooks.Update(maturitaBook);
        }
    }
}
