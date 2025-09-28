using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IGenreRepository
    {
        Task<Genre?> GetByIdAsync(int id);
        Task<Genre?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<Genre>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<Genre>> GetAllAsync();
        void Add(Genre genre);
        void Update(Genre genre);
        void Delete(Genre genre);
    }
}
