using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateMaturitaBookUseCase
    {
        Task<GenericResponse> AddToAvailableAsync(Guid bookPublicId);
    }
}
