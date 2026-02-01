using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IPublisherRepository
    {
        Task<Publisher?> GetByIdAsync(int id);
        Task<Publisher?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<Publisher>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25);
        Task<PagedResult<Publisher>> GetPagedAsync(int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<Publisher>> GetAllAsync();
        void Add(Publisher publisher);
        void Update(Publisher publisher);
        void Delete(Publisher publisher);
    }
}
