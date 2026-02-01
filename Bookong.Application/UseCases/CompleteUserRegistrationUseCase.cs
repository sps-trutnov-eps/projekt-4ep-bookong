using System;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;

namespace Bookong.Application.UseCases
{
    public class CompleteUserRegistrationUseCase : ICompleteUserRegistrationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompleteUserRegistrationUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<CompleteUserRegistrationResponse> ExecuteAsync(CompleteUserRegistrationRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Name))
            {
                return new CompleteUserRegistrationResponse
                {
                    UserId = Guid.Empty,
                    Message = "Username and Name are required",
                    Success = false
                };
            }

            // Check if user already exists
            var existingUser = await _unitOfWork.Users.GetBySamAccountNameAsync(request.Username);
            if (existingUser != null)
            {
                return new CompleteUserRegistrationResponse
                {
                    UserId = existingUser.PublicId,
                    Message = "User already exists",
                    Success = true,
                    Username = existingUser.SamAccountName,
                    Name = existingUser.Name,
                    Email = existingUser.Email,
                    OrganizationalUnit = existingUser.OrganizationalUnit
                };
            }

            // Create new user
            var user = new User
            {
                SamAccountName = request.Username,
                ObjectGuid = Guid.NewGuid(),
                Name = request.Name,
                Email = $"{request.Username}@spstrutnov.cz",
                OrganizationalUnit = "Default"
            };

            _unitOfWork.Users.Add(user);
            await _unitOfWork.CommitAsync();

            return new CompleteUserRegistrationResponse
            {
                UserId = user.PublicId,
                Message = "User registered successfully",
                Success = true,
                Username = user.SamAccountName,
                Name = user.Name,
                Email = user.Email,
                OrganizationalUnit = user.OrganizationalUnit
            };
        }
    }
}
