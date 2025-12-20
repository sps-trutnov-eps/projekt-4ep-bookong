using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Bookong.Infrastructure.Services;
using System.IO;
using System.Drawing;
using System.Linq;

namespace Bookong.Web.Controllers
{
    public class QrController : Controller
    {
        private readonly BookService _books;

        public QrController()
        {
            _books = new BookService();
        }

        [HttpGet("/qr/{isbn}")]
        public IActionResult Generate(string isbn)
        {
            var list = _books.LoadBooks();
            var book = list.FirstOrDefault(b => b.ISBN == isbn);

            if (book == null)
                return NotFound("Kniha nenalezena");

            string qrText = $"{book.Nazev}\n{book.Stoleti}\n{book.Autor}\n{book.ISBN}";

            QRCodeGenerator gen = new QRCodeGenerator();
            QRCodeData data = gen.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            QRCode qr = new QRCode(data);

            using var bmp = qr.GetGraphic(20);
            using var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return File(ms.ToArray(), "image/png");
        }

        [HttpGet("/qr/title/{title}")]
        public IActionResult GenerateByTitle(string title)
        {
            var list = _books.LoadBooks();
            var book = list.FirstOrDefault(b => b.Nazev.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (book == null)
                return NotFound("Kniha nenalezena");

            string qrText = $"{book.Nazev}\n{book.Stoleti}\n{book.Autor}\n{book.ISBN}";

            QRCodeGenerator gen = new QRCodeGenerator();
            QRCodeData data = gen.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            QRCode qr = new QRCode(data);

            using var bmp = qr.GetGraphic(20);
            using var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return File(ms.ToArray(), "image/png");
        }
    }
}
