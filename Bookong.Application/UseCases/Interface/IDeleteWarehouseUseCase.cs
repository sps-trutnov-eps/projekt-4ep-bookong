using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface IDeleteWarehouseUseCase
    {
        Task<GenericResponse> HandleAsync(DeleteWarehouseDto dto);
    }
}
