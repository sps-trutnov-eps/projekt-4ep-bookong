using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByIdAsync(int id);
        Task<PagedResult<Author>> GetByNameAsync(string name, int pageNumber = 1, int pageSize = 25);
        Task<PagedResult<Author>> GetPagedAsync(int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<Author>> GetAllAsync();
        void Add(Author author);
        void Update(Author author);
        void Delete(Author author);
    }
}
