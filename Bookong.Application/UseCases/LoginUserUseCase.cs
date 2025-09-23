using Bookong.Application.DTOs;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    public class LoginUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public LoginUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginUserResponse> Handle(LoginUserRequest request)
        {
            // TODO: Implement login logic (authentication, eventually registration, etc.)
        }
    }
}
