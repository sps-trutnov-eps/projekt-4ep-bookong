using System;
using System.Linq;
using System.Threading.Tasks;
using Bookong.Application.UseCases.Commands.Interfaces;
using Bookong.Domain.Entities;
using Bookong.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases.Commands
{
    public class AddBookToMaturitaSelectionCommand : IAddBookToMaturitaSelectionCommand
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddBookToMaturitaSelectionCommand> _logger;

        public AddBookToMaturitaSelectionCommand(IUnitOfWork unitOfWork, ILogger<AddBookToMaturitaSelectionCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task ExecuteAsync(Guid bookPublicId)
        {
            try
            {
                // Get the first user from seeded data
                var users = await _unitOfWork.Users.GetAllAsync();
                var user = users.FirstOrDefault()
                    ?? throw new InvalidOperationException("No users found in database. Please run seed data first.");

                // Get the book by its public ID
                var book = await _unitOfWork.Books.GetByPublicIdAsync(bookPublicId)
                    ?? throw new ArgumentException($"Book with PublicId {bookPublicId} not found", nameof(bookPublicId));

                // Check if the book is already in the selection
                var allSelections = await _unitOfWork.MaturitaBookSelections.GetAllAsync();
                var exists = allSelections.Any(m => m.BookId == book.Id && m.UserId == user.Id);

                if (!exists)
                {
                    _logger.LogInformation("Adding book {BookId} to maturita selection for user {UserId}", book.Id, user.Id);

                    var selection = new MaturitaBookSelection
                    {
                        BookId = book.Id,
                        UserId = user.Id
                    };

                    _unitOfWork.MaturitaBookSelections.Add(selection);
                    await _unitOfWork.CommitAsync();

                    _logger.LogInformation("Book {BookId} successfully added to maturita selection for user {UserId}", book.Id, user.Id);
                }
                else
                {
                    _logger.LogInformation("Book {BookId} is already in maturita selection for user {UserId}", book.Id, user.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding book {BookPublicId} to maturita selection", bookPublicId);
                throw;
            }
        }
    }
}