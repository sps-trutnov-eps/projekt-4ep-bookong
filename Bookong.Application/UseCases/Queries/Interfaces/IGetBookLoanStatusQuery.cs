using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetBookLoanStatusQuery
    {
        Task<BookLoanStatusDto?> ExecuteAsync(Guid bookPublicId);
    }
}
