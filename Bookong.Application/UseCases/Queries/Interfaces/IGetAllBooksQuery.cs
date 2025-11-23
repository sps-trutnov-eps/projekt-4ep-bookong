using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetAllBooksQuery
    {
        Task<IReadOnlyList<BookListItemDto>> ExecuteAsync();
    }
}