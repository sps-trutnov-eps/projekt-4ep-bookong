using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IUpdateBookUseCase
    {
        Task<GenericResponse> ExecuteAsync(UpdateBookDto dto);
    }
}
