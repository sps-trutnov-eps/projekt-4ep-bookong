using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetAvailableBooksQuery
    {
        Task<IReadOnlyList<BookListItemDto>> ExecuteAsync();
    }
}
