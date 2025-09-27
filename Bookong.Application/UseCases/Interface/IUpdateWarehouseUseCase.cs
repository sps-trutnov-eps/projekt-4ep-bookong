using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface IUpdateWarehouseUseCase
    {
        Task<GenericResponse> HandleAsync(UpdateWarehouseDto dto);
    }
}
