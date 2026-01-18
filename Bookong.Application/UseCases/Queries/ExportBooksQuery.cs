using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;
using ClosedXML.Excel;
using QRCoder;


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
                Name = b.Name ?? "Bez názvu",
                Author = b.Author != null ? $"{b.Author.FirstName} {b.Author.LastName}" : "Neznámý autor",
                ISBN = b.ISBN ?? "Bez ISBN",
                Genre = b.Genre?.Name ?? "Nezařazeno",
                Kind = b.Kind?.Name ?? "Nezařazeno",
                Period = b.Period?.Name ?? "Nezařazeno",
                Pages = b.Pages.HasValue ? (int)b.Pages.Value : 0,
                DateRelease = b.DateRelease,
                Warehouse = b.Warehouse?.Name ?? "Nezařazeno"
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
            ws.Cell(1, 10).Value = "QR kód";
            ws.Cell(1, 11).Value = "Veřejné ID";

            var header = ws.Range(1, 1, 1, 11);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.LightGray;

            int row = 2;
            foreach (var b in data)
            {
                ws.Cell(row, 1).Value = b.Name ?? "Bez názvu";
                ws.Cell(row, 2).Value = b.Author ?? "Neznámý autor";
                ws.Cell(row, 3).Value = b.ISBN ?? "Bez ISBN";
                ws.Cell(row, 4).Value = b.Genre ?? "Nezařazeno";
                ws.Cell(row, 5).Value = b.Kind ?? "Nezařazeno";
                ws.Cell(row, 6).Value = b.Period ?? "Nezařazeno";
                ws.Cell(row, 7).Value = b.Pages > 0 ? b.Pages : 0;
                ws.Cell(row, 8).Value = b.DateRelease?.ToString("dd.MM.yyyy") ?? "Neznámé datum";
                ws.Cell(row, 9).Value = b.Warehouse ?? "Nezařazeno";
                
                try
                {
                    using (var qrGenerator = new QRCodeGenerator())
                    {
                        var qrCodeData = qrGenerator.CreateQrCode(b.Id.ToString(), QRCodeGenerator.ECCLevel.Q);
                        using var qrCode = new PngByteQRCode(qrCodeData);
                        var qrCodeImage = qrCode.GetGraphic(5);

                        using var stream = new MemoryStream(qrCodeImage);
                        stream.Position = 0;
                        var picture = ws.AddPicture(stream)
                            .MoveTo(ws.Cell(row, 10))
                            .WithSize(100, 100);
                    }

                    ws.Row(row).Height = 75;
                }
                catch (Exception ex)
                {
                    ws.Cell(row, 10).Value = $"Chyba QR: {ex.Message}";
                }
                
                ws.Cell(row, 11).Value = b.Id.ToString();
                row++;
            }

            ws.Column(10).Width = 15;
            ws.Columns(1, 9).AdjustToContents();
            ws.Column(11).AdjustToContents();

            using var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<byte[]> ExportAllToExcelAsync()
        {
            int[] books = await _uow.Books.GetAllIdsAsync();

            return await ExportToExcelAsync(books);
        }


    }
}
