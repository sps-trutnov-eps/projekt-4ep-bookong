using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetBookDetailQuery
    {
        Task<BookDetailDto?> ExecuteAsync(Guid publicId);
    }
}
