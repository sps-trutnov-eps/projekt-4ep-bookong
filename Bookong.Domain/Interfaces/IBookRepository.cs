using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Queries;

namespace Bookong.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<Book>> GetFilteredAsync(BookSearchCriteria criteria, int pageNumber = 1, int pageSize = 25);
        Task<PagedResult<Book>> GetPagedAsync(int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<Book>> GetAllAsync();
        void Add(Book book);
        void Update(Book book);
        void Delete(Book book);
    }
}
