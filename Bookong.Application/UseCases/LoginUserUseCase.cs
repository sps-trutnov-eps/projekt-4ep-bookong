using System;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Application.Services.Interfaces;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginUserUseCase(IActiveDirectoryService activeDirectoryService, IUnitOfWork unitOfWork)
        {
            _activeDirectoryService = activeDirectoryService ?? throw new ArgumentNullException(nameof(activeDirectoryService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<LoginUserResponse> ExecuteAsync(LoginUserRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            // Extract username from email (before @)
            var username = request.Email.Contains('@') 
                ? request.Email.Split('@')[0] 
                : request.Email;

            // Validate credentials via Active Directory service
            var isValid = await _activeDirectoryService.ValidateCredentialsAsync(username, request.Password);

            if (!isValid)
            {
                return new LoginUserResponse
                {
                    UserId = Guid.Empty,
                    Message = "Invalid credentials",
                    IsFirstTimeUser = false,
                    Username = username
                };
            }

            // Check if user exists in database
            var user = await _unitOfWork.Users.GetBySamAccountNameAsync(username);

            if (user == null)
            {
                // First-time user - return success but flag for additional info collection
                return new LoginUserResponse
                {
                    UserId = Guid.Empty,
                    Message = "First time login - please provide your name",
                    IsFirstTimeUser = true,
                    Username = username
                };
            }

            // Existing user - login successful
            return new LoginUserResponse
            {
                UserId = user.PublicId,
                Message = "Login successful",
                IsFirstTimeUser = false,
                Username = username,
                Name = user.Name,
                Email = user.Email,
                OrganizationalUnit = user.OrganizationalUnit
            };
        }
    }
}
