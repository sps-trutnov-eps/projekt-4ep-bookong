using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IKindRepository
    {
        Task<Kind?> GetByIdAsync(int id);
        Task<Kind?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<Kind>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<Kind>> GetAllAsync();
        void Add(Kind kind);
        void Update(Kind kind);
        void Delete(Kind kind);
    }
}
