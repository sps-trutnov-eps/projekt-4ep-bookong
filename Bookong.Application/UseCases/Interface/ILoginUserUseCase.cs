using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface ILoginUserUseCase
    {
        Task<LoginUserResponse> HandleAsync(LoginUserRequest request);
    }
}
