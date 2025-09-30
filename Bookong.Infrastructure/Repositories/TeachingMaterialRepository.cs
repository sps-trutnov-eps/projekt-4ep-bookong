using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class TeachingMaterialRepository(BookongDbContext context) : ITeachingMaterialRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(TeachingMaterial teachingMaterial)
        {
            throw new NotImplementedException();
        }

        public void Delete(TeachingMaterial teachingMaterial)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TeachingMaterial>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TeachingMaterial?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TeachingMaterial?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<TeachingMaterial>> GetByTitleAsync(string title, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<TeachingMaterial>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(TeachingMaterial teachingMaterial)
        {
            throw new NotImplementedException();
        }
    }
}
