using System;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Application.Services.Interfaces;

namespace Bookong.Application.UseCases
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly IActiveDirectoryService _activeDirectoryService;

        // Store email only for optional debugging; do not store password
        public string? StoredEmail { get; private set; }

        public LoginUserUseCase(IActiveDirectoryService activeDirectoryService)
        {
            _activeDirectoryService = activeDirectoryService ?? throw new ArgumentNullException(nameof(activeDirectoryService));
        }

        public async Task<LoginUserResponse> ExecuteAsync(LoginUserRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            // Keep only email for debugging; do NOT persist password
            StoredEmail = request.Email;

            // Validate credentials via Active Directory service
            var isValid = await _activeDirectoryService.ValidateCredetialsAsync(request.Email, request.Password);

            if (!isValid)
            {
                return new LoginUserResponse
                {
                    UserId = Guid.Empty,
                    Message = "Invalid credentials"
                };
            }

            // If valid, try to fetch additional user details
            var details = await _activeDirectoryService.GetUserDetailsAsync(request.Email);

            if (details is null)
            {
                return new LoginUserResponse
                {
                    UserId = Guid.Empty,
                    Message = "User validated but details not found"
                };
            }

            return new LoginUserResponse
            {
                UserId = details.Value.ObjectGuid,
                Message = "Login successful"
            };
        }
    }
}
