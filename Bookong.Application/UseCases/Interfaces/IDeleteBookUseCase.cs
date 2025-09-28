using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IDeleteBookUseCase
    {
        Task<GenericResponse> HandleAsync(int id);
    }
}
