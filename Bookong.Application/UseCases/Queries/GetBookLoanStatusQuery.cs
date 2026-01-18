using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases.Queries
{
    public class GetBookLoanStatusQuery : IGetBookLoanStatusQuery
    {
        private readonly IUnitOfWork _uow;

        public GetBookLoanStatusQuery(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<BookLoanStatusDto?> ExecuteAsync(Guid bookPublicId)
        {
            var book = await _uow.Books.GetByPublicIdAsync(bookPublicId);
            
            if (book == null)
                return null;

            var allLoans = await _uow.BookLoans.GetAllAsync();
            var activeLoan = allLoans.FirstOrDefault(bl => 
                bl.BookId == book.Id && 
                bl.ReturnDate == null);

            return new BookLoanStatusDto
            {
                BookId = book.Id,
                BookPublicId = book.PublicId,
                BookTitle = book.Name,
                BookAuthor = $"{book.Author.FirstName} {book.Author.LastName}",
                Genre = book.Genre.Name,
                IsCurrentlyLoaned = activeLoan != null,
                CurrentLoanId = activeLoan?.Id,
                CurrentBorrowerName = activeLoan?.User.Name,
                LoanDate = activeLoan?.LoanDate,
                DaysLoaned = activeLoan != null 
                    ? (DateTime.UtcNow - activeLoan.LoanDate).Days 
                    : null
            };
        }
    }
}
