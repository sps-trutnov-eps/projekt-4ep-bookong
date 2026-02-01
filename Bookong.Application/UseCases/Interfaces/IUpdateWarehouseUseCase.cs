using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IUpdateWarehouseUseCase
    {
        Task<GenericResponse> ExecuteAsync(UpdateWarehouseDto dto);
    }
}
