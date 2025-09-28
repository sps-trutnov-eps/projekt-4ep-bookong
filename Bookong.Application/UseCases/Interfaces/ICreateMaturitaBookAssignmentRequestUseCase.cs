using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateMaturitaBookAssignmentRequestUseCase
    {
        Task<GenericResponse> HandleAsync(CreateMaturitaBookAssignmentRequestDto dto);
    }
}
