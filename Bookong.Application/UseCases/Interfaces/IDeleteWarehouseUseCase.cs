using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IDeleteWarehouseUseCase
    {
        Task<GenericResponse> ExecuteAsync(DeleteWarehouseDto dto);
    }
}
