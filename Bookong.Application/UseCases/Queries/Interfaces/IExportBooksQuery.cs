using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces;

public interface IExportBooksQuery
{
    Task<IEnumerable<BookExportDto>> ExecuteAsync(int[] ids);
    Task<byte[]> ExportToExcelAsync(int[] ids);
    Task<byte[]> ExportAllToExcelAsync();
}
