using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IExportBookQrCodesQueryV2
    {
        Task<IEnumerable<BookQrCodeDto>> ExecuteAsync(int[] ids);
        Task<byte[]> ExportToExcelAsync(int[] ids);
        Task<byte[]> ExportToExcelByPublicIdsAsync(Guid[] publicIds);
        Task<byte[]> ExportAllToExcelAsync();
    }
}
