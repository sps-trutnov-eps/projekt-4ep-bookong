using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure.Repositories
{
    public class BookReservationRepository(BookongDbContext context) : IBookReservationRepository
    {
        private readonly BookongDbContext _context = context;

        public void Add(BookReservation bookReservation)
        {
            throw new NotImplementedException();
        }

        public void Delete(BookReservation bookReservation)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookReservation>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BookReservation?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BookReservation?> GetByPublicIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<BookReservation>> GetPagedAsync(int pageNumber = 1, int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public void Update(BookReservation bookReservation)
        {
            throw new NotImplementedException();
        }
    }
}
