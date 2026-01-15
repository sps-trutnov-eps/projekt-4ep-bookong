using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ICompleteUserRegistrationUseCase
    {
        Task<CompleteUserRegistrationResponse> ExecuteAsync(CompleteUserRegistrationRequest request);
    }
}
