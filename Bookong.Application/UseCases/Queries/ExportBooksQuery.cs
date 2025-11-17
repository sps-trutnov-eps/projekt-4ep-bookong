using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;
using ClosedXML.Excel;


namespace Bookong.Application.UseCases.Queries
{
    public class ExportBooksQuery : IExportBooksQuery
    {
        private readonly IUnitOfWork _uow;

        public ExportBooksQuery(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<BookExportDto>> ExecuteAsync(int[] ids)
        {
            var books = (await _uow.Books.GetByIdsAsync(ids)).ToList();

            return books.Select(b => new BookExportDto
            {
                Id = b.PublicId,
                Name = b.Name,
                Author = b.Author != null ? $"{b.Author.FirstName} {b.Author.LastName}" : string.Empty,
                ISBN = b.ISBN ?? string.Empty,
                Genre = b.Genre?.Name ?? string.Empty,
                Kind = b.Kind?.Name ?? string.Empty,
                Period = b.Period?.Name ?? string.Empty,
                Pages = (int)b.Pages,
                DateRelease = b.DateRelease,
                Warehouse = b.Warehouse?.Name ?? string.Empty
            }).ToList();
        }

        public async Task<byte[]> ExportToExcelAsync(int[] ids)
        {
            var data = await ExecuteAsync(ids);

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Knihy");

            ws.Cell(1, 1).Value = "Název knihy";
            ws.Cell(1, 2).Value = "Autor";
            ws.Cell(1, 3).Value = "ISBN";
            ws.Cell(1, 4).Value = "Žánr";
            ws.Cell(1, 5).Value = "Druh";
            ws.Cell(1, 6).Value = "Období";
            ws.Cell(1, 7).Value = "Počet stran";
            ws.Cell(1, 8).Value = "Datum vydání";
            ws.Cell(1, 9).Value = "Sklad";

            var header = ws.Range(1, 1, 1, 9);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;
            foreach (var b in data)
            {
                ws.Cell(row, 1).Value = b.Name;
                ws.Cell(row, 2).Value = b.Author;
                ws.Cell(row, 3).Value = b.ISBN;
                ws.Cell(row, 4).Value = b.Genre;
                ws.Cell(row, 5).Value = b.Kind;
                ws.Cell(row, 6).Value = b.Period;
                ws.Cell(row, 7).Value = b.Pages;
                ws.Cell(row, 8).Value = b.DateRelease?.ToString("dd.MM.yyyy");
                ws.Cell(row, 9).Value = b.Warehouse;
                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<byte[]> ExportAllToExcelAsync()
        {
            int[] books = await _uow.Books.GetAllIdsAsync();

            return await ExportToExcelAsync(books);
        }


    }
}
