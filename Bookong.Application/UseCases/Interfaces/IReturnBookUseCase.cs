using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IReturnBookUseCase
    {
        Task<GenericResponse> ExecuteAsync(Guid bookPublicId);
    }
}
