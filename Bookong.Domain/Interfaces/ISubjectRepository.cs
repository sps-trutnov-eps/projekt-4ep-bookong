
using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface ISubjectRepository
    {
        Task<Subject?> GetByIdAsync(int id);
        Task<Subject?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<Subject>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<Subject>> GetAllAsync();
        void Add(Subject subject);
        void Update(Subject subject);
        void Delete(Subject subject);
    }
}
