using Bookong.Application.DTOs;

public interface IExportBooksQuery
{
    Task<IEnumerable<BookExportDto>> ExecuteAsync(int[] ids);
    Task<byte[]> ExportToExcelAsync(int[] ids);

    Task<byte[]> ExportAllToExcelAsync();

}
