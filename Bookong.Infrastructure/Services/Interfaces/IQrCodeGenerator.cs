namespace Bookong.Infrastructure.Services.Interfaces
{
    public interface IQrCodeGenerator
    {
        byte[] GeneratePng<T>(IEnumerable<T> data) where T : class;
    }
}
