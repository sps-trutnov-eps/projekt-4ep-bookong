using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface ICreateMaturitaBookAssignmentRequestUseCase
    {
        Task<GenericResponse> HandleAsync(CreateMaturitaBookAssignmentRequestDto dto);
    }
}
