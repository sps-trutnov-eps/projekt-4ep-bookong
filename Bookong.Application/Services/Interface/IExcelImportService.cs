using Bookong.Application.DTOs;

namespace Bookong.Application.Services.Interface
{
    public interface IExcelImportService
    {
        public List<ImportBookDto> ParseBooks(Stream excelStream);
    }
}
