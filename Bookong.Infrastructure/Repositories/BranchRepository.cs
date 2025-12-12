
using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class BranchRepository(BookongDbContext context) : IBranchRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(Branch branch)
        {
            throw new NotImplementedException();
        }

        public void Delete(Branch branch)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Branch>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Branch?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Branch>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<Branch?> GetByPublicIdAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<Branch>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(Branch branch)
        {
            throw new NotImplementedException();
        }
    }
}
