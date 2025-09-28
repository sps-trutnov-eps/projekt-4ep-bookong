using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateMaturitaBookUseCase
    {
        Task<GenericResponse> HandleAsync(CreateMaturitaBookDto dto);
    }
}
