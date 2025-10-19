using Bookong.Application.DTOs;
using Bookong.Application.Services;
using Bookong.Application.UseCases.Interfaces;
using System.Collections.Generic;

namespace Bookong.Application.UseCases
{
    public class ImportBooksFromExcelUseCase : IImportBooksFromExcelUseCase
    {
        public Task<GenericResponse> ExecuteAsync(Stream excelStream)
        {
            // Uzivatel nahraje ten excel
            ExcelImportService excelImportService = new ExcelImportService();
            List<ImportBookDto> KnizkyzExccelu = excelImportService.ParseBooks(excelStream);
            // Cteme ten excel
            throw new NotImplementedException();
        }
    }
}
