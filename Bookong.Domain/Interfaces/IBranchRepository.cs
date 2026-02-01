using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IBranchRepository
    {
        Task<Branch?> GetByIdAsync(int id);
        Task<Branch?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<Branch>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25);
        Task<PagedResult<Branch>> GetPagedAsync(int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<Branch>> GetAllAsync();
        void Add(Branch branch);
        void Update(Branch branch);
        void Delete(Branch branch);
    }
}
