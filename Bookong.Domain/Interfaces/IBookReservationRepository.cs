using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IBookReservationRepository
    {
        Task<BookReservation?> GetByIdAsync(int id);
        Task<BookReservation?> GetByPublicIdAsync(string id);
        Task<PagedResult<BookReservation>> GetPagedAsync(int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<BookReservation>> GetAllAsync();
        void Add(BookReservation bookReservation);
        void Update(BookReservation bookReservation);
        void Delete(BookReservation bookReservation);
    }
}
