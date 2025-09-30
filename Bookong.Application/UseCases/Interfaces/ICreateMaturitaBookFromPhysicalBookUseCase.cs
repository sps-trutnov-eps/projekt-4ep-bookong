using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateMaturitaBookFromPhysicalBookUseCase
    {
        Task<GenericResponse> ExecuteAsync(int physicalBookId);
    }
}
