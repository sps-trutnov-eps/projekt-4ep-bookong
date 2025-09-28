using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateBookReservationUseCase
    {
        Task<GenericResponse> ExecuteAsync(CreateBookReservationDto createReservationDto);
    }
}
