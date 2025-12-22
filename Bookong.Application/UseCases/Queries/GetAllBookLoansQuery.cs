using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases.Queries
{
    public class GetAllBookLoansQuery : IGetAllBookLoansQuery
    {
        private readonly IUnitOfWork _uow;

        public GetAllBookLoansQuery(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<BookLoanListItemDto>> ExecuteAsync()
        {
            var loans = await _uow.BookLoans.GetAllAsync();

            var result = loans
                .Select(bl => new BookLoanListItemDto
                {
                    PublicId = bl.PublicId,
                    BookTitle = bl.Book.Name,
                    BookAuthor = $"{bl.Book.Author.FirstName} {bl.Book.Author.LastName}",
                    UserFullName = bl.User.Name,
                    UserEmail = bl.User.Email,
                    LoanDate = bl.LoanDate,
                    ReturnDate = bl.ReturnDate
                })
                .OrderByDescending(bl => bl.LoanDate)
                .ToList();

            return result;
        }
    }
}
