using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface ICreateWarehouseUseCase
    {
        Task<GenericResponse> HandleAsync(CreateWarehouseDto dto);
    }
}
