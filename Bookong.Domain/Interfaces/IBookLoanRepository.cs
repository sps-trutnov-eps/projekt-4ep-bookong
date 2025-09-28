using Bookong.Domain.Common.Models;
using Bookong.Domain.Entities;

namespace Bookong.Domain.Interfaces
{
    public interface IBookLoanRepository
    {
        Task<BookLoan?> GetByIdAsync(int id);
        Task<BookLoan?> GetByPublicIdAsync(Guid publicId);
        Task<PagedResult<BookLoan>> GetByUserAsync(User user, int pageNumber = 1, int pageSize = 25);
        Task<PagedResult<BookLoan>> GetPagedAsync(int pageNumber = 1, int pageSize = 25);
        Task<IEnumerable<BookLoan>> GetAllAsync();
        void Add(BookLoan bookLoan);
        void Update(BookLoan bookLoan);
        void Delete(BookLoan bookLoan);
    }
}
