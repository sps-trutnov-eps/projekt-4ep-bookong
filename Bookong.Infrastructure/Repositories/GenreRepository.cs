using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class GenreRepository(BookongDbContext context) : IGenreRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Genre genre)
        {
            throw new NotImplementedException();
        }

        public void Delete(Genre genre)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Genre>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Genre?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Genre>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<Genre?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public void Update(Genre genre)
        {
            throw new NotImplementedException();
        }
    }
}
