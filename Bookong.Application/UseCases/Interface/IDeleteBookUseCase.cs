using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface IDeleteBookUseCase
    {
        Task<GenericResponse> HandleAsync(int bookId);
    }
}
