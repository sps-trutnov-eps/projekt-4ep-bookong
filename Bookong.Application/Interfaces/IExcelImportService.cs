using Bookong.Application.DTOs;

namespace Bookong.Application.Interfaces
{
    public interface IExcelImportService
    {
        public List<ImportBookDto> ParseBooks(Stream excelStream);
    }
}
