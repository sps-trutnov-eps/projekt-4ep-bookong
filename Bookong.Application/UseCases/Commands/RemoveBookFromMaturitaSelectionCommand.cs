using System;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.Services.Interfaces;
using Bookong.Application.UseCases.Commands.Interfaces;
using Bookong.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases.Commands
{
    public class RemoveBookFromMaturitaSelectionCommand : IRemoveBookFromMaturitaSelectionCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionService _sessionService;
        private readonly ILogger<RemoveBookFromMaturitaSelectionCommand> _logger;

        public RemoveBookFromMaturitaSelectionCommand(
            IUnitOfWork unitOfWork,
            ISessionService sessionService,
            ILogger<RemoveBookFromMaturitaSelectionCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _sessionService = sessionService;
            _logger = logger;
        }

        public async Task ExecuteAsync(Guid bookPublicId)
        {
            try
            {
                // Pokusíme se získat CurrentUserPublicId z relace
                var user = (await TryGetUserFromSessionAsync()) ?? throw new InvalidOperationException("No users found in database.");

                // Získáme knihu podle publicId přes repository
                var book = await _unitOfWork.Books.GetByPublicIdAsync(bookPublicId)
                    ?? throw new ArgumentException($"Book with PublicId {bookPublicId} not found.", nameof(bookPublicId));

                // Najdeme odpovídající výběr
                var selections = await _unitOfWork.MaturitaBookSelections.GetAllAsync();
                var selection = selections.FirstOrDefault(m => m.BookId == book.Id && m.UserId == user.Id);

                if (selection != null)
                {
                    _unitOfWork.MaturitaBookSelections.Delete(selection);
                    await _unitOfWork.CommitAsync();

                    _logger.LogInformation("Removed book {BookId} from maturita selection for user {UserId}", book.Id, user.Id);
                }
                else
                {
                    _logger.LogInformation("Book {BookId} was not found in maturita selection for user {UserId}", book.Id, user.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing book {BookPublicId} from maturita selection", bookPublicId);
                throw;
            }
        }

        private async Task<Domain.Entities.User?> TryGetUserFromSessionAsync()
        {
            try
            {
                var currentUserPublicId = await _sessionService.GetAsync("CurrentUserPublicId");
                _logger.LogDebug("Session CurrentUserPublicId = {CurrentUserPublicId}", currentUserPublicId);

                if (!string.IsNullOrWhiteSpace(currentUserPublicId) && Guid.TryParse(currentUserPublicId, out var publicId))
                {
                    var userFromSession = await _unitOfWork.Users.GetByPublicIdAsync(publicId);
                    if (userFromSession != null)
                        return userFromSession;

                    _logger.LogDebug("User with PublicId {PublicId} not found, will fallback to seeded user.", publicId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error reading CurrentUserPublicId from session, will fallback to seeded user.");
            }

            // fallback na seedovaného uživatele
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.FirstOrDefault();
        }
    }
}