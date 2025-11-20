using System;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Application.Services.Interfaces;
using Bookong.Domain.Interfaces;
using Bookong.Domain.Entities;


namespace Bookong.Application.UseCases
{
    public class LoginUserUseCase : ILoginUserUseCase
    {
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IUnitOfWork _unitOfWork;

        // Store email only for optional debugging; do not store password
        public string? StoredEmail { get; private set; }

        public LoginUserUseCase(IActiveDirectoryService activeDirectoryService, IUnitOfWork unitOfWork)
        {
            _activeDirectoryService = activeDirectoryService ?? throw new ArgumentNullException(nameof(activeDirectoryService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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

            var users = await _unitOfWork.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.ObjectGuid == details.Value.ObjectGuid);

            if (user == null)
            {
                user = new User
                {
                    SamAccountName = request.Email.Split('@')[0],
                    ObjectGuid = details.Value.ObjectGuid,
                    Name = details.Value.Name,
                    Email = details.Value.Email,
                    OrganizationalUnit = await _activeDirectoryService.GetUserOranizationalUnitAsync(request.Email) ?? "Unknown"
                };

                _unitOfWork.Users.Add(user);
                await _unitOfWork.CommitAsync();
            }

            return new LoginUserResponse
            {
                UserId = user.PublicId,
                Message = "Login successful"
            };
        }
    }
}
