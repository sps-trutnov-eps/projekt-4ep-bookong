using System;
using System.Threading.Tasks;
using Bookong.Application.UseCases.Commands.Interfaces;
using Bookong.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bookong.Application.UseCases.Commands
{
    public class RemoveBookFromMaturitaSelectionCommand : IRemoveBookFromMaturitaSelectionCommand
    {
        private readonly BookongDbContext _dbContext;
        private readonly ILogger<RemoveBookFromMaturitaSelectionCommand> _logger;

        public RemoveBookFromMaturitaSelectionCommand(
            BookongDbContext dbContext,
            ILogger<RemoveBookFromMaturitaSelectionCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task ExecuteAsync(Guid bookPublicId)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync()
                    ?? throw new InvalidOperationException("No users found in database.");

                var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.PublicId == bookPublicId)
                    ?? throw new ArgumentException($"Book with PublicId {bookPublicId} not found.");

                var selection = await _dbContext.MaturitaBookSelections
                    .FirstOrDefaultAsync(m => m.BookId == book.Id && m.UserId == user.Id);

                if (selection != null)
                {
                    _dbContext.MaturitaBookSelections.Remove(selection);
                    await _dbContext.SaveChangesAsync();
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