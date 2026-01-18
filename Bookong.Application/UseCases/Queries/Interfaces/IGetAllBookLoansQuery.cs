using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetAllBookLoansQuery
    {
        Task<IReadOnlyList<BookLoanListItemDto>> ExecuteAsync();
    }
}
