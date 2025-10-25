using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;

namespace Bookong.Application.UseCases
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        // Store credentials in variables for later AD authentication check
        public string? StoredEmail { get; private set; }
        public string? StoredPassword { get; private set; }

        public Task<LoginUserResponse> ExecuteAsync(LoginUserRequest request)
        {
            // Store the credentials that user entered
            StoredEmail = request.Email;
            StoredPassword = request.Password;

            // Return response with debug message
            var response = new LoginUserResponse
            {
                UserId = Guid.Empty,
                Message = "Credentials in debug"
            };

            return Task.FromResult(response);
        }
    }
}
