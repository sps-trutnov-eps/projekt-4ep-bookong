using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IReturnBookUseCase
    {
        Task<GenericResponse> HandleAsync(Guid bookPublicId);
    }
}
