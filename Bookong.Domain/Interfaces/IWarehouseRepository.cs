using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<Warehouse?> GetByIdAsync(int id);
        Task<Warehouse?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<Warehouse>> GetPagedAsync(int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<Warehouse>> GetAllAsync();
        void Add(Warehouse warehouse);
        void Update(Warehouse warehouse);
        void Delete(Warehouse warehouse);
    }
}
