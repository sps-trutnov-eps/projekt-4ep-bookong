using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface IManageMaturitaBookAvailabilityUseCase
    {
        Task<OperationResult> AddToAvailableAsync(Guid bookPublicId);
        Task<OperationResult> RemoveFromAvailableAsync(Guid bookPublicId);
    }
}