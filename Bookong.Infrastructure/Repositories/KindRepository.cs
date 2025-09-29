using Bookong.Domain.Common.Models;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class KindRepository(BookongDbContext context) : IKindRepository
    {
        private readonly BookongDbContext _context = context;
        public void Add(Domain.Entities.Kind kind)
        {
            throw new NotImplementedException();
        }
        public void Delete(Domain.Entities.Kind kind)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Domain.Entities.Kind>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Domain.Entities.Kind?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<PagedResult<Domain.Entities.Kind>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }
        public Task<Domain.Entities.Kind?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }
        public void Update(Domain.Entities.Kind kind)
        {
            throw new NotImplementedException();
        }
    }
}
