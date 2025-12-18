
using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class SubjectRepository(BookongDbContext context) : ISubjectRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Subject subject)
        {
            throw new NotImplementedException();
        }

        public void Delete(Subject subject)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Subject>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Subject?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Subject>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<Subject?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public void Update(Subject subject)
        {
            throw new NotImplementedException();
        }
    }
}
