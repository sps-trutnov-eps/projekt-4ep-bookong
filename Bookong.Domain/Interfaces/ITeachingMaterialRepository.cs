using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface ITeachingMaterialRepository
    {
        Task<TeachingMaterial?> GetByIdAsync(int id);
        Task<TeachingMaterial?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<TeachingMaterial>> GetByTitleAsync(string title, int pageNumber = 1, int pageSize = 25);
        Task<PagedResult<TeachingMaterial>> GetPagedAsync(int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<TeachingMaterial>> GetAllAsync();
        void Add(TeachingMaterial teachingMaterial);
        void Update(TeachingMaterial teachingMaterial);
        void Delete(TeachingMaterial teachingMaterial);
    }
}
