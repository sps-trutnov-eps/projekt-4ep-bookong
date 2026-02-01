using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateMaturitaBookAssignmentRequestUseCase
    {
        Task<GenericResponse> ExecuteAsync(CreateMaturitaBookAssignmentRequestDto dto);
    }
}
