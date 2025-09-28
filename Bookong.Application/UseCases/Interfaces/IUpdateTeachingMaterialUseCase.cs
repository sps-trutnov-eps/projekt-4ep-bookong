using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IUpdateTeachingMaterialUseCase
    {
        Task<GenericResponse> HandleAsync(UpdateTeachingMaterialDto dto);
    }
}
