using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Queries;

namespace Bookong.Domain.Interfaces
{
    public interface IMaturitaBookRepository
    {
        Task<MaturitaBook?> GetByIdAsync(int id);
        Task<MaturitaBook?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<MaturitaBook>> GetFilteredAsync(BookSearchCriteria criteria, int pageNumber = 1, int pageSize = 25);
        Task<PagedResult<MaturitaBook>> GetAllAsync();
        Task<IEnumerable<MaturitaBook>> GetAllWithDetailsAsync();
        Task<MaturitaBook?> FindExistingAsync(string name, int authorId, int genreId, int kindId, int periodId);
        void Add(MaturitaBook maturitaBook);
        void Update(MaturitaBook maturitaBook);
        void Delete(MaturitaBook maturitaBook);
    }
}
