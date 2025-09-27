using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface IUpdateBookUseCase
    {
        Task<GenericResponse> HandleAsync(UpdateBookDto dto);
    }
}
