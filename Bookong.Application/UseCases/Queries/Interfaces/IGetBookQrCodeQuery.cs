using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetBookQrCodeQuery
    {
        Task<BookQrCodeRequestDto> HandleAsync(int[] id);
    }
}
