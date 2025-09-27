using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface ICreateMaturitaBookUseCase
    {
        Task<GenericResponse> HandleAsync(CreateMaturitaBookDto dto);
    }
}
