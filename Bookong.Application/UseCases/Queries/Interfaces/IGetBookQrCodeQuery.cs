using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Queries.Interfaces
{
    public interface IGetBookQrCodeQuery
    {
        Task<BookQrCodeRequestDto> ExecuteAsync(int[] id);
    }
}
