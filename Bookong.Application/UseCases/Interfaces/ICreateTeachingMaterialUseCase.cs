using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateTeachingMaterialUseCase
    {
        Task<GenericResponse> HandleAsync(CreateTeachingMaterialDto dto);
    }
}
