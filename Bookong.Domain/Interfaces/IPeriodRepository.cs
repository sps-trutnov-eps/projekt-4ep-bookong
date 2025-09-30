using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IPeriodRepository
    {
        Task<Period?> GetByIdAsync(int id);
        Task<Period?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<Period>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<Period>> GetAllAsync();
        void Add(Period period);
        void Update(Period period);
        void Delete(Period period);
    }
}
