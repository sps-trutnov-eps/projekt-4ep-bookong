using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface ICreateMaturitaBookFromPhysicalBookUseCase
    {
        Task<GenericResponse> HandleAsync(int physicalBookId);
    }
}
