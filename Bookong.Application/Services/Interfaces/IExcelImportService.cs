using Bookong.Application.DTOs;

namespace Bookong.Application.Services.Interfaces
{
    public interface IExcelImportService
    {
        public List<ImportBookDto> ParseBooks(Stream excelStream);
    }
}
