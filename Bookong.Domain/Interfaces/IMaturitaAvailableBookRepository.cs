using Bookong.Domain.Entities;
using Bookong.Domain.Common.Models;

namespace Bookong.Domain.Interfaces
{
    public interface IMaturitaAvailableBookRepository
    {
        Task<IEnumerable<MaturitaAvailableBook>> GetAllAsync();
        Task<MaturitaAvailableBook?> GetByIdAsync(int id);
        Task<IEnumerable<MaturitaAvailableBook>> GetByBookIdAsync(int bookId);
        Task<IEnumerable<MaturitaAvailableBook>> GetByUserIdAsync(int userId);
        void Add(MaturitaAvailableBook entity);
        void Delete(MaturitaAvailableBook entity);
    }
}