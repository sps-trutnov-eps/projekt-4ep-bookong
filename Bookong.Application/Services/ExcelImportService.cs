using Bookong.Application.DTOs;
using Bookong.Application.Services.Interfaces;
using ClosedXML.Excel;

namespace Bookong.Application.Services
{
    internal class ExcelImportService : IExcelImportService
    {
        public List<ImportBookDto> ParseBooks(Stream excelStream)
        {
            var books = new List<ImportBookDto>();

            using var workbook = new XLWorkbook(excelStream);
            var worksheet = workbook.Worksheets.First();

            // Assuming first row is header
            var rows = worksheet.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                var book = new ImportBookDto
                {
                    Title = row.Cell(1).GetString(),
                    ISBN = row.Cell(2).GetString(),
                    AuthorName = row.Cell(3).GetString(),
                    GenreName = row.Cell(4).GetString(),
                    KindName = row.Cell(5).GetString(),
                    PeriodName = row.Cell(6).GetString(),
                    Pages = ushort.TryParse(row.Cell(7).GetString(), out var pages) ? pages : (ushort)0,
                    PublisherName = row.Cell(8).GetString(),
                    DateRelease = DateTime.TryParse(row.Cell(9).GetString(), out var date) ? date : null,
                    WarehouseName = row.Cell(10).GetString()
                };

                books.Add(book);
            }

            return books;
        }
    }
}
