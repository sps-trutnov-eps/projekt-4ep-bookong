using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICreateWarehouseUseCase
    {
        Task<GenericResponse> HandleAsync(CreateWarehouseDto dto);
    }
}
