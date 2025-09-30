using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Domain.Queries;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class MaturitaBookSelectionRepository(BookongDbContext context) : IMaturitaBookSelectionRepository
    {
        private readonly BookongDbContext _context = context;

        public Task<IEnumerable<MaturitaBookSelection>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MaturitaBookSelection?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<MaturitaBookSelection>> GetByUserAsync(User user, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<MaturitaBookSelection>> GetByUserFilteredAsync(User user, BookSearchCriteria criteria, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }
    }
}
