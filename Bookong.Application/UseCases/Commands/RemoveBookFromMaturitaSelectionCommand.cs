using System;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.UseCases.Commands.Interfaces;
using Bookong.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases.Commands
{
    public class RemoveBookFromMaturitaSelectionCommand : IRemoveBookFromMaturitaSelectionCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveBookFromMaturitaSelectionCommand> _logger;

        public RemoveBookFromMaturitaSelectionCommand(
            IUnitOfWork unitOfWork,
            ILogger<RemoveBookFromMaturitaSelectionCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ExecuteAsync(Guid bookPublicId)
        {
            try
            {
                // Získáme prvního uživatele ze seedovaných dat přes repository
                var users = await _unitOfWork.Users.GetAllAsync();
                var user = users.FirstOrDefault()
                    ?? throw new InvalidOperationException("No users found in database.");

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
    }
}