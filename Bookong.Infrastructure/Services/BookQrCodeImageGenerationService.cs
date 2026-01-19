using Bookong.Domain.Interfaces;
using QRCoder;
using System.Drawing.Imaging;

namespace Bookong.Infrastructure.Services
{
    public class BookQrCodeImageGenerationService : IBookQrCodeImageGenerationService
    {
        public byte[] GenerateQrCodeImage(Guid bookPublicId)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(bookPublicId.ToString(), QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new QRCode(qrCodeData);
            using var qrCodeImage = qrCode.GetGraphic(10);

            using var ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
