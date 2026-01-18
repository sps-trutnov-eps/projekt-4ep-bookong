namespace Bookong.Domain.Interfaces
{
    public interface IBookQrCodeImageGenerationService
    {
        byte[] GenerateQrCodeImage(Guid bookPublicId);
    }
}
