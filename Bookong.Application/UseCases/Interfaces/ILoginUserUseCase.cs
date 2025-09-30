using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interfaces
{
    public interface ILoginUserUseCase
    {
        Task<LoginUserResponse> ExecuteAsync(LoginUserRequest request);
    }
}
