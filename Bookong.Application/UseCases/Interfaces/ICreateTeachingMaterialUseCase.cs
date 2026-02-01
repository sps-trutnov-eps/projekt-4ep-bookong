using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateTeachingMaterialUseCase
    {
        Task<GenericResponse> ExecuteAsync(CreateTeachingMaterialDto dto);
    }
}
