using System;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.DTOs;
using Bookong.Application.Services.Interfaces;
using Bookong.Application.UseCases.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases
{
    public class SelectMaturitaBookUseCase : ISelectMaturitaBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<SelectMaturitaBookUseCase> _logger;

        public SelectMaturitaBookUseCase(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<SelectMaturitaBookUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<GenericResponse> Handle(AddBookToPersonalMaturitaListDto dto)
        {
            try
            {
                // Get user from authentication state (if present)
                User? user = null;
                try
                {
                    var currentUserPublicId = await _currentUserService.GetCurrentUserPublicIdAsync();
                    _logger.LogDebug("Current user PublicId = {CurrentUserPublicId}", currentUserPublicId);

                    if (currentUserPublicId.HasValue)
                    {
                        user = await _unitOfWork.Users.GetByPublicIdAsync(currentUserPublicId.Value);

                        if (user == null)
                        {
                            _logger.LogDebug("User with PublicId {PublicId} not found, will fallback to seeded user.", currentUserPublicId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error reading current user identity, will fallback to seeded user.");
                }

                // Fallback to seeded user if no user in session
                if (user is null)
                {
                    var users = await _unitOfWork.Users.GetAllAsync();
                    user = users.FirstOrDefault();

                    if (user is null)
                    {
                        _logger.LogError("No users found in database. Please run seed data first.");
                        return GenericResponse.FailureResponse("No users found in database. Please run seed data first.", "USER_NOT_FOUND");
                    }

                    _logger.LogDebug("Using fallback seeded user Id={UserId}", user.Id);
                }

                // Get the book by its public ID
                var book = await _unitOfWork.Books.GetByPublicIdAsync(dto.BookPublicId);

                if (book is null)
                {
                    _logger.LogWarning("Book with PublicId {BookPublicId} not found", dto.BookPublicId);
                    return GenericResponse.FailureResponse($"Book with PublicId {dto.BookPublicId} not found", "BOOK_NOT_FOUND");
                }

                // Check if the book is already in the selection
                var allSelections = await _unitOfWork.MaturitaBookSelections.GetAllAsync();
                var exists = allSelections.Any(m => m.BookId == book.Id && m.UserId == user.Id);

                if (exists)
                {
                    _logger.LogInformation("Book {BookId} is already in maturita selection for user {UserId}", book.Id, user.Id);
                    return GenericResponse.SuccessResponse("Book is already in your maturita selection");
                }

                _logger.LogInformation("Adding book {BookId} to maturita selection for user {UserId}", book.Id, user.Id);

                var selection = new MaturitaBookSelection
                {
                    BookId = book.Id,
                    UserId = user.Id
                };

                _unitOfWork.MaturitaBookSelections.Add(selection);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Book {BookId} successfully added to maturita selection for user {UserId}", book.Id, user.Id);
                return GenericResponse.SuccessResponse("Book successfully added to your maturita selection");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book {BookPublicId} to maturita selection", dto.BookPublicId);
                return GenericResponse.FailureResponse("An error occurred while adding the book to your maturita selection", "INTERNAL_ERROR");
            }
        }
    }
}
