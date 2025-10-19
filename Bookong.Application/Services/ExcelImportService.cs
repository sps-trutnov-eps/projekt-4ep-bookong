using Bookong.Application.DTOs;
using Bookong.Application.Services.Interfaces;

namespace Bookong.Application.Services
{
    internal class ExcelImportService : IExcelImportService
    {
        public List<ImportBookDto> ParseBooks(Stream excelStream)
        {
            // This service will handle parsing the excel file and returning list of ImportBookDto

            // This needs to be implemented in order for the ImportBooksFromExcelUseCase to work

            throw new NotImplementedException();
        }
    }
}
