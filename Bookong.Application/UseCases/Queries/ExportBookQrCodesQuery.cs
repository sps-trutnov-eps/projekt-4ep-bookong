using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Queries.Interfaces;
using Bookong.Domain.Interfaces;
using ClosedXML.Excel;

namespace Bookong.Application.UseCases.Queries
{
    public class ExportBookQrCodesQuery : IExportBookQrCodesQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly IBookQrCodeImageGenerationService _qrCodeService;

        public ExportBookQrCodesQuery(IUnitOfWork uow, IBookQrCodeImageGenerationService qrCodeService)
        {
            _uow = uow;
            _qrCodeService = qrCodeService;
        }

        public async Task<IEnumerable<BookQrCodeDto>> ExecuteAsync(int[] ids)
        {
            var books = (await _uow.Books.GetByIdsAsync(ids)).ToList();

            return books.Select(b => new BookQrCodeDto
            {
                InternalId = b.Id,
                PublicId = b.PublicId,
                Name = b.Name,
                QrCodeImage = _qrCodeService.GenerateQrCodeImage(b.PublicId)
            }).ToList();
        }

        public async Task<byte[]> ExportToExcelAsync(int[] ids)
        {
            var data = await ExecuteAsync(ids);
            return GenerateExcelFromData(data);
        }

        public async Task<byte[]> ExportToExcelByPublicIdsAsync(Guid[] publicIds)
        {
            var allBooks = await _uow.Books.GetAllAsync();
            var books = allBooks.Where(b => publicIds.Contains(b.PublicId)).ToList();

            var data = books.Select(b => new BookQrCodeDto
            {
                InternalId = b.Id,
                PublicId = b.PublicId,
                Name = b.Name,
                QrCodeImage = _qrCodeService.GenerateQrCodeImage(b.PublicId)
            }).ToList();

            return GenerateExcelFromData(data);
        }

        public async Task<byte[]> ExportAllToExcelAsync()
        {
            int[] bookIds = await _uow.Books.GetAllIdsAsync();
            return await ExportToExcelAsync(bookIds);
        }

        private byte[] GenerateExcelFromData(IEnumerable<BookQrCodeDto> data)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("QR Kódy knih");

            // Headers
            ws.Cell(1, 1).Value = "ID knihy";
            ws.Cell(1, 2).Value = "Název knihy";
            ws.Cell(1, 3).Value = "QR kód";
            ws.Cell(1, 4).Value = "PublicId";

            var header = ws.Range(1, 1, 1, 4);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.LightBlue;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Set column widths
            ws.Column(1).Width = 10;
            ws.Column(2).Width = 40;
            ws.Column(3).Width = 20;
            ws.Column(4).Width = 40;

            int row = 2;
            foreach (var book in data)
            {
                // Internal ID (database ID)
                ws.Cell(row, 1).Value = book.InternalId;
                ws.Cell(row, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // Book name
                ws.Cell(row, 2).Value = book.Name;
                ws.Cell(row, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // QR Code image
                try
                {
                    using var qrStream = new MemoryStream(book.QrCodeImage);
                    var picture = ws.AddPicture(qrStream)
                .MoveTo(ws.Cell(row, 3))
                    .Scale(0.45);

                    ws.Row(row).Height = 70;
                }
                catch (Exception ex)
                {
                    ws.Cell(row, 3).Value = $"Error: {ex.Message}";
                }

                // Public ID (for QR code reference)
                ws.Cell(row, 4).Value = book.PublicId.ToString();
                ws.Cell(row, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
