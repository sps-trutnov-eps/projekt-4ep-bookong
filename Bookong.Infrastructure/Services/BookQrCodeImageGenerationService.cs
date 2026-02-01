using Bookong.Domain.Interfaces;
using QRCoder;

namespace Bookong.Infrastructure.Services
{
    public class BookQrCodeImageGenerationService : IBookQrCodeImageGenerationService
    {
        public byte[] GenerateQrCodeImage(Guid bookPublicId)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(bookPublicId.ToString(), QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(10);
        }
    }
}
