using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bookong.Infrastructure.Repositories
{
    public class GenreRepository(BookongDbContext context) : IGenreRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Genre genre)
        {
            _context.Genres.Add(genre);
        }

        public void Delete(Genre genre)
        {
            _context.Genres.Remove(genre);
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await _context.Genres
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Genre?> GetByPublicIdAsync(Guid publicId)
        {
            return await _context.Genres
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.PublicId == publicId);
        }

        public async Task<PagedResult<Genre>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            var query = _context.Genres
                .AsNoTracking()
                .Where(g => g.Name.Contains(name))
                .OrderBy(g => g.Name);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Genre>(items, totalCount, pageNumber, pageSize);
        }

        public void Update(Genre genre)
        {
            _context.Genres.Update(genre);
        }
    }
}
