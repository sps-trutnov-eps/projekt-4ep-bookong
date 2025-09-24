using Bookong.Application.DTOs;

namespace Bookong.Application.UseCases.Interface
{
    public interface ILoginUserUseCase
    {
        public Task<LoginUserResponse> Handle(LoginUserRequest request);
    }
}
