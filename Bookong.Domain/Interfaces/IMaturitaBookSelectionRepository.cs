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

        // Přidáno: metody pro přidání / odstranění výběru
        void Add(MaturitaBookSelection maturitaBookSelection);
        void Delete(MaturitaBookSelection maturitaBookSelection);

        // Volitelně: metoda pro načtení výběru s navázanými detaily
        Task<IEnumerable<MaturitaBookSelection>> GetByUserWithDetailsAsync(int userId);
    }
}
