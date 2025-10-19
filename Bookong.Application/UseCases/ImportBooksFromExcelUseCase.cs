using Bookong.Application.DTOs;
using Bookong.Application.Services;
using Bookong.Application.UseCases.Interfaces;
using System.Collections.Generic;

namespace Bookong.Application.UseCases
{
    public class ImportBooksFromExcelUseCase : IImportBooksFromExcelUseCase
    {
        public async Task<GenericResponse> ExecuteAsync(Stream excelStream)
        {
            // Parse Excel file to get books
            var excelImportService = new ExcelImportService();
            List<ImportBookDto> booksFromExcel;

            try
            {
                // Run the parsing on a background thread to avoid blocking
                booksFromExcel = await Task.Run(() => excelImportService.ParseBooks(excelStream));
            }
            catch (Exception ex)
            {
                return GenericResponse.FailureResponse(
                    message: $"Failed to import books: {ex.Message}",
                    statusCode: "IMPORT_ERROR"
                );
            }

            if (booksFromExcel == null || booksFromExcel.Count == 0)
            {
                return GenericResponse.FailureResponse(
                    message: "No books found in the uploaded Excel file.",
                    statusCode: "NO_DATA"
                );
            }

            // TODO: Save books to database here if needed

            return GenericResponse.SuccessResponse(
                message: $"Successfully imported {booksFromExcel.Count} books.",
                payload: booksFromExcel
            );
        }
    }
}
