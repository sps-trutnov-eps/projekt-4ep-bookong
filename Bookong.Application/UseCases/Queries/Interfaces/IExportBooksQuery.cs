using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IExportBooksQuery
    {
        Task<IEnumerable<BookExportDto>> HandleAsync(int[] ids);
    }
}
