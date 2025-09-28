using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IDeleteTeachingMaterialUseCase
    {
        Task<GenericResponse> ExecuteAsync(int id);
    }
}
