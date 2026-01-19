using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetUserBookLoansQuery
    {
        Task<IReadOnlyList<BookLoanListItemDto>> ExecuteAsync(string samAccountName);
    }
}
