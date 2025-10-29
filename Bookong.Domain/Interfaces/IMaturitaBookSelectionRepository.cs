using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Queries;

namespace Bookong.Domain.Interfaces
{
    public interface IMaturitaBookSelectionRepository
    {
        Task<MaturitaBookSelection?> GetByIdAsync(int id);
        Task<PagedResult<MaturitaBookSelection>> GetByUserAsync(User user, int pageNumber = 1, int pageSize = 25);
        Task<PagedResult<MaturitaBookSelection>> GetByUserFilteredAsync(User user, BookSearchCriteria criteria, int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<MaturitaBookSelection>> GetAllAsync();

        // Added: methods for adding / removing a selection
        void Add(MaturitaBookSelection maturitaBookSelection);
        void Delete(MaturitaBookSelection maturitaBookSelection);

        // Optional: method to load selection with related details
        Task<IEnumerable<MaturitaBookSelection>> GetByUserWithDetailsAsync(int userId);
    }
}
