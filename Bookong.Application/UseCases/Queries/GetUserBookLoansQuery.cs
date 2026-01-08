using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases.Queries
{
    public class GetUserBookLoansQuery : IGetUserBookLoansQuery
    {
        private readonly IUnitOfWork _uow;

        public GetUserBookLoansQuery(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IReadOnlyList<BookLoanListItemDto>> ExecuteAsync(string samAccountName)
        {
            var user = await _uow.Users.GetBySamAccountNameAsync(samAccountName);

            if (user == null)
            {
                return new List<BookLoanListItemDto>();
            }

            var pagedResult = await _uow.BookLoans.GetByUserAsync(user, 1, 1000);

            var result = pagedResult.Items
                .Select(bl => new BookLoanListItemDto
                {
                    PublicId = bl.PublicId,
                    BookTitle = bl.Book.Name,
                    BookAuthor = $"{bl.Book.Author.FirstName} {bl.Book.Author.LastName}",
                    BookPeriod = bl.Book.Period.Name,
                    BookKind = bl.Book.Kind.Name,
                    BookISBN = bl.Book.ISBN,
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
